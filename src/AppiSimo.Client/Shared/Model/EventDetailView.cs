namespace AppiSimo.Client.Shared.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using AppiSimo.Shared.Model;
    using AppiSimo.Shared.Validators.Abstract;
    using AppiSimo.Shared.Validators.Model;

    public class EventDetailView : IViewModel<Event>
    {
        readonly IValidator<Event> _validator;
        public Event Event { get; }

        public ICollection<UserEvent> SelectedUserEvents { get; set; } = new List<UserEvent>();

        public ICollection<UserEvent> FilteredUserEvents { get; set; } = new List<UserEvent>();

        public ICollection<Court> Courts { get; } = new List<Court>();

        public ICollection<Light> Lights { get; } = new List<Light>();

        public ICollection<Heat> Heats { get; } = new List<Heat>();

        // Validation Properties
        public bool IsValidStarDate { get; set; }
        public bool IsValidEndDate { get; set; }

        public Result State { get; set; } = Result.Valid;

        public EventDetailView()
        {
            Event = new Event();
        }

        public EventDetailView(Event @event, IEnumerable<User> users, IEnumerable<Court> courts, IEnumerable<Light> lights, IEnumerable<Heat> heats, IValidator<Event> validator)
        {
            Event = @event;

            _validator = validator;

            Lights = lights.ToList();
            Heats = heats.ToList();
            Courts = courts.ToList();

            SelectedCourt = @event.CourtId == Guid.Empty ? Courts.OrderBy(court => court.Name).First().Id.ToString() : @event.CourtId.ToString();

            SelectedUserEvents = @event.UsersEvents;
            FilteredUserEvents = users.Where(user => !SelectedUserEvents.Select(sue => sue.UserId).Contains(user.Id)).Select(user => new UserEvent { EventId = @event.Id, UserId = user.Id, User = user }).ToList();

            IsValidStarDate = IsValidEndDate = !IsNewEntity();
        }

        public bool IsNewEntity() => Event.Id == Guid.Empty;

        public string SelectedStartDate
        {
            get => Event.StartDate == DateTime.MinValue ? string.Empty : Event.StartDate.ToLocalTime().ToString("g");
            set
            {
                IsValidStarDate = DateTime.TryParse(value, out var datetime);

                Event.StartDate = datetime.ToUniversalTime();
                SetLightAndHeatDuration();
                State = _validator.Validate(Event);
            }
        }

        public string SelectedEndDate
        {
            get => Event.EndDate == DateTime.MinValue ? string.Empty : Event.EndDate.ToLocalTime().ToString("g");
            set
            {
                IsValidEndDate = DateTime.TryParse(value, out var datetime);

                Event.EndDate = datetime.ToUniversalTime();
                SetLightAndHeatDuration();
                State = _validator.Validate(Event);
            }
        }

        public string SelectedCourt { get => Event.CourtId.ToString(); set => Event.CourtId = Guid.Parse(value); }

        // TODO: https://github.com/aspnet/Blazor/issues/576
        public string SelectedLight
        {
            get => Event.LightId?.ToString();
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Event.LightId = null;
                }
                else
                {
                    Event.LightId = Guid.Parse(value);
                }
            }
        }

        public string SelectedLightDuration
        {
            get => Event.LightDuration.ToString("0.0");
            set
            {
                var isValidDuration = double.TryParse(value, out var duration);
                Event.LightDuration = isValidDuration ? duration : 0;
            }
        }

        public string SelectedHeat
        {
            get => Event.HeatId?.ToString();
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Event.HeatId = null;
                }
                else
                {
                    Event.HeatId = Guid.Parse(value);
                }
            }
        }

        public string SelectedHeatDuration
        {
            get => Event.HeatDuration.ToString("0.0");
            set
            {
                var isValidDuration = double.TryParse(value, out var duration);
                Event.HeatDuration = isValidDuration ? duration : 0;
            }
        }

        double GetDuration()
        {
            double duration = 0;

            if (!IsValidEndDate || !IsValidEndDate)
            {
                return duration;
            }

            duration = (Event.EndDate - Event.StartDate).TotalMinutes > 0 ? (Event.EndDate - Event.StartDate).TotalMinutes : 0;

            return duration;
        }

        void SetLightAndHeatDuration()
        {
            Event.LightDuration = Event.HeatDuration = GetDuration() / 60;
        }

        void Validate()
        {
        }
    }
}