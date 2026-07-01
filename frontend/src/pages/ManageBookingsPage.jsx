import React, { useEffect, useState } from 'react';
import { api } from '../api/client';
import { useAuth } from '../context/AuthContext';
import BoardingPass from '../components/BoardingPass';

export default function ManageBookingsPage() {
  const { user } = useAuth();
  const [bookings, setBookings] = useState([]);
  const [reference, setReference] = useState('');
  const [lookupResult, setLookupResult] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    if (user) {
      api.getMyBookings(user.userId).then(setBookings).catch(() => setBookings([]));
    }
  }, [user]);

  const handleLookup = async (e) => {
    e.preventDefault();
    setError('');
    setLookupResult(null);
    try {
      const booking = await api.getBookingByReference(reference.trim().toUpperCase());
      setLookupResult(booking);
    } catch (err) {
      setError('No booking found with that reference.');
    }
  };

  const handleCancel = async (id) => {
    if (!window.confirm('Cancel this booking? This cannot be undone.')) return;
    await api.cancelBooking(id);
    setBookings((prev) => prev.map((b) => (b.id === id ? { ...b, status: 'Cancelled' } : b)));
  };

  return (
    <div className="page">
      <h1 className="page-title">My trips</h1>

      <form className="lookup-form" onSubmit={handleLookup}>
        <label className="field">
          <span>Look up a booking by reference</span>
          <input
            value={reference}
            onChange={(e) => setReference(e.target.value)}
            placeholder="e.g. AB12CD"
          />
        </label>
        <button className="btn btn--primary" type="submit">Find booking</button>
      </form>

      {error && <p className="alert alert--error">{error}</p>}
      {lookupResult && (
        <div className="lookup-result">
          <BoardingPass booking={lookupResult} />
        </div>
      )}

      {user ? (
        <section className="trip-list">
          <h2>Booked under your account</h2>
          {bookings.length === 0 && <p>No bookings yet — go search for a flight!</p>}
          {bookings.map((b) => (
            <div className="trip-row" key={b.id}>
              <div>
                <strong>{b.flight.flightNumber}</strong> · {b.flight.departureAirportCode} &#8594; {b.flight.arrivalAirportCode}
                <div className="trip-row__meta">Ref {b.bookingReference} · {b.status}</div>
              </div>
              {b.status !== 'Cancelled' && (
                <button className="btn btn--ghost" onClick={() => handleCancel(b.id)}>Cancel</button>
              )}
            </div>
          ))}
        </section>
      ) : (
        <p className="page-subtitle">Sign in to see all bookings linked to your account.</p>
      )}
    </div>
  );
}
