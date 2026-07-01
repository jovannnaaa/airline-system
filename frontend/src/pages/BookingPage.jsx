import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { api } from '../api/client';
import { useAuth } from '../context/AuthContext';
import PassengerForm from '../components/PassengerForm';

const emptyPassenger = (seatClass) => ({
  firstName: '', lastName: '', passportNumber: '', dateOfBirth: '', email: '', phone: '', seatClass,
});

export default function BookingPage() {
  const { flightId } = useParams();
  const location = useLocation();
  const navigate = useNavigate();
  const { user } = useAuth();

  const seatClass = location.state?.seatClass || 'Economy';
  const passengerCount = location.state?.passengers || 1;

  const [flight, setFlight] = useState(null);
  const [passengers, setPassengers] = useState(
    Array.from({ length: passengerCount }, () => emptyPassenger(seatClass))
  );
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    api.getFlight(flightId).then(setFlight).catch((err) => setError(err.message));
  }, [flightId]);

  const updatePassenger = (index, updated) => {
    const next = [...passengers];
    next[index] = updated;
    setPassengers(next);
  };

  const totalPrice = flight
    ? passengers.reduce((sum) => {
        const price = seatClass === 'Business' ? flight.businessPrice
          : seatClass === 'FirstClass' ? flight.firstClassPrice
          : flight.economyPrice;
        return sum + price;
      }, 0)
    : 0;

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    setError('');
    try {
      const booking = await api.createBooking({
        flightId: Number(flightId),
        userId: user?.userId ?? null,
        passengers: passengers.map((p) => ({ ...p, seatClass })),
      });
      navigate(`/confirmation/${booking.id}`);
    } catch (err) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  };

  if (!flight) {
    return <div className="page"><p>{error || 'Loading flight…'}</p></div>;
  }

  return (
    <div className="page">
      <div className="booking-layout">
        <form className="booking-form" onSubmit={handleSubmit}>
          <h1 className="page-title">Passenger details</h1>
          <p className="page-subtitle">
            {flight.flightNumber} · {flight.departureAirportCode} &#8594; {flight.arrivalAirportCode}
          </p>

          {passengers.map((p, i) => (
            <PassengerForm key={i} index={i} passenger={p} onChange={updatePassenger} />
          ))}

          {error && <p className="alert alert--error">{error}</p>}

          <button className="btn btn--primary btn--block" type="submit" disabled={submitting}>
            {submitting ? 'Confirming…' : `Confirm booking · $${totalPrice.toFixed(2)}`}
          </button>
        </form>

        <aside className="booking-summary">
          <h2>Trip summary</h2>
          <dl>
            <dt>Route</dt>
            <dd>{flight.departureCity} ({flight.departureAirportCode}) &#8594; {flight.arrivalCity} ({flight.arrivalAirportCode})</dd>
            <dt>Cabin</dt>
            <dd>{seatClass === 'FirstClass' ? 'First class' : seatClass}</dd>
            <dt>Passengers</dt>
            <dd>{passengerCount}</dd>
            <dt>Total</dt>
            <dd className="booking-summary__total">${totalPrice.toFixed(2)}</dd>
          </dl>
        </aside>
      </div>
    </div>
  );
}
