import React from 'react';
import { useNavigate } from 'react-router-dom';

const CLASS_LABELS = {
  Economy: 'Economy',
  Business: 'Business',
  FirstClass: 'First class',
};

function formatTime(iso) {
  return new Date(iso).toLocaleString(undefined, {
    weekday: 'short', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit',
  });
}

function duration(depart, arrive) {
  const ms = new Date(arrive) - new Date(depart);
  const hours = Math.floor(ms / 3.6e6);
  const minutes = Math.round((ms % 3.6e6) / 6e4);
  return `${hours}h ${minutes}m`;
}

function priceForClass(flight, seatClass) {
  if (seatClass === 'Business') return flight.businessPrice;
  if (seatClass === 'FirstClass') return flight.firstClassPrice;
  return flight.economyPrice;
}

export default function FlightList({ flights, seatClass, passengers }) {
  const navigate = useNavigate();

  if (flights.length === 0) {
    return (
      <div className="empty-state">
        <p className="empty-state__title">No flights match this search.</p>
        <p>Try a different date or route — Skyward Air adds new routes every season.</p>
      </div>
    );
  }

  return (
    <ul className="flight-list">
      {flights.map((f) => (
        <li key={f.id} className="flight-card">
          <div className="flight-card__route">
            <div className="flight-card__endpoint">
              <span className="flight-card__code">{f.departureAirportCode}</span>
              <span className="flight-card__time">{formatTime(f.departureTimeUtc)}</span>
            </div>
            <div className="flight-card__path">
              <span className="flight-card__duration">{duration(f.departureTimeUtc, f.arrivalTimeUtc)}</span>
              <div className="flight-card__line"><span className="flight-card__plane">&#9992;</span></div>
              <span className="flight-card__flightno">{f.flightNumber} · {f.airplaneModel}</span>
            </div>
            <div className="flight-card__endpoint">
              <span className="flight-card__code">{f.arrivalAirportCode}</span>
              <span className="flight-card__time">{formatTime(f.arrivalTimeUtc)}</span>
            </div>
          </div>

          <div className="flight-card__book">
            <div className="flight-card__price">
              <span className="flight-card__price-amount">${priceForClass(f, seatClass).toFixed(2)}</span>
              <span className="flight-card__price-label">{CLASS_LABELS[seatClass]} · per passenger</span>
            </div>
            <button
              className="btn btn--primary"
              onClick={() => navigate(`/book/${f.id}`, { state: { seatClass, passengers } })}
            >
              Select
            </button>
          </div>
        </li>
      ))}
    </ul>
  );
}
