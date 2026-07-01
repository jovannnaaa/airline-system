import React from 'react';

const CLASS_LABELS = {
  Economy: 'ECONOMY',
  Business: 'BUSINESS',
  FirstClass: 'FIRST',
};

function formatDate(iso) {
  return new Date(iso).toLocaleDateString(undefined, { weekday: 'short', month: 'short', day: 'numeric' });
}
function formatTime(iso) {
  return new Date(iso).toLocaleTimeString(undefined, { hour: '2-digit', minute: '2-digit' });
}

export default function BoardingPass({ booking }) {
  const { flight } = booking;

  return (
    <div className="boarding-pass">
      <div className="boarding-pass__stub">
        <div className="boarding-pass__brand">SKYWARD AIR</div>

        <div className="boarding-pass__route">
          <div className="boarding-pass__airport">
            <span className="boarding-pass__code">{flight.departureAirportCode}</span>
            <span className="boarding-pass__city">{flight.departureCity}</span>
          </div>
          <span className="boarding-pass__arrow">&#10230;</span>
          <div className="boarding-pass__airport">
            <span className="boarding-pass__code">{flight.arrivalAirportCode}</span>
            <span className="boarding-pass__city">{flight.arrivalCity}</span>
          </div>
        </div>

        <div className="boarding-pass__grid">
          <div><span className="boarding-pass__label">Flight</span><strong>{flight.flightNumber}</strong></div>
          <div><span className="boarding-pass__label">Date</span><strong>{formatDate(flight.departureTimeUtc)}</strong></div>
          <div><span className="boarding-pass__label">Boards</span><strong>{formatTime(flight.departureTimeUtc)}</strong></div>
          <div><span className="boarding-pass__label">Reference</span><strong>{booking.bookingReference}</strong></div>
        </div>

        <div className="boarding-pass__passengers">
          {booking.passengers.map((p, i) => (
            <div className="boarding-pass__passenger" key={i}>
              <span>{p.firstName} {p.lastName}</span>
              <span className="boarding-pass__seatclass">{CLASS_LABELS[p.seatClass] || p.seatClass}</span>
            </div>
          ))}
        </div>
      </div>

      <div className="boarding-pass__perforation" aria-hidden="true" />

      <div className="boarding-pass__pocket">
        <span className="boarding-pass__pocket-code">{flight.departureAirportCode}</span>
        <span className="boarding-pass__pocket-arrow">&#8594;</span>
        <span className="boarding-pass__pocket-code">{flight.arrivalAirportCode}</span>
        <span className="boarding-pass__pocket-ref">{booking.bookingReference}</span>
      </div>
    </div>
  );
}
