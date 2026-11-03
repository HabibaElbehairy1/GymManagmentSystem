using AutoMapper;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Booking;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.MemberShip;
using GymManagementSystem.BLL.ViewModels.Sessions;
using BookingEntity = GymManagement.DAL.Models.Booking;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // cancel
        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAcync(sessionId, ct);
            if (session is null)
                return Result.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result.Validation("Cannot cancel a booking for a session that has already started.");

            var booking = await _unitOfWork.BookingRepository
                .FirstOrDefaultAsync(b => b.SessionId == sessionId && b.MemberId == memberId, ct);
            if (booking is null)
                return Result.NotFound("Booking not found.");

            _unitOfWork.BookingRepository.Delete(booking);
            return await _unitOfWork.SaveChangesAsync(ct) > 0
                ? Result.Ok()
                : Result.Fail("Booking cancel failed.", ResultKind.Conflict);
        }

        // attendance
        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository
                .FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, ct);
            if (booking is null)
                return Result.NotFound("Booking not found.");

            booking.IsAttended = true;
            booking.LastUpdatedAt = DateTime.UtcNow;
            _unitOfWork.BookingRepository.Update(booking);

            return await _unitOfWork.SaveChangesAsync(ct) > 0
                ? Result.Ok()
                : Result.Fail("Failed to mark as attended.", ResultKind.Conflict);
        }

        // create
        public async Task<Result> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAcync(model.SessionId, ct);
            if (session is null)
                return Result.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result.Validation("Cannot book a session that has already started.");

            var hasActiveMembership = await _unitOfWork.MembershipRepository
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (!hasActiveMembership)
                return Result.Validation("Member does not have an active membership.");

            var alreadyBooked = await _unitOfWork.BookingRepository
                .AnyAsync(b => b.SessionId == model.SessionId && b.MemberId == model.MemberId, ct);
            if (alreadyBooked)
                return Result.Validation("Member is already booked for this session.");

            var booked = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(model.SessionId, ct);
            if (booked >= session.Capacity)
                return Result.Validation("Session is full.");

            _unitOfWork.BookingRepository.Add(new BookingEntity
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                IsAttended = false,
            });

            return await _unitOfWork.SaveChangesAsync(ct) > 0
                ? Result.Ok()
                : Result.Fail("Failed to book session.", ResultKind.Conflict);
        }

        // sessions list (upcoming + ongoing)
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var sessions = await _unitOfWork.SessionRepository
                .GetAllSessionsWithTrainerAndCategory(ct);

            var current = sessions.Where(s => s.EndDate >= now);

            // AvailableSlots بيتحسب في الـ Mapping (Capacity - Members.Count)
            return _mapper.Map<IEnumerable<SessionViewModel>>(current);
        }

        // members of an upcoming session
        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpcomingBySessionIdAsync(
            int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBySessionIdAsync(sessionId, ct);

            return bookings.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = sessionId,
                MemberName = b.Member.Name,
                BookingDate = b.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
            }).ToList();
        }

        // members of an ongoing session
        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOngoingBySessionIdAsync(
            int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBySessionIdAsync(sessionId, ct);

            return bookings.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = sessionId,
                MemberName = b.Member.Name,
                BookingDate = b.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                IsAttended = b.IsAttended,
            }).ToList();
        }

        // dropdown: members not yet booked in this session
        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(
            int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAcync(ct: ct);
            var bookedMemberIds = bookings
                .Where(x => x.SessionId == sessionId)
                .Select(x => x.MemberId)
                .ToList();

            var allMembers = await _unitOfWork.GetRepository<Member>().GetAllAcync(ct: ct);
            var members = allMembers.Where(x => !bookedMemberIds.Contains(x.Id));

            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }
    }
}