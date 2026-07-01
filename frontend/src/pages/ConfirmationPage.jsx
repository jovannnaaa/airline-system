import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { api } from '../api/client';
import BoardingPass from '../components/BoardingPass';

export default function ConfirmationPage() {
  const { bookingId } = useParams();
  const [booking, setBooking] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    api.getBooking(bookingId).then(setBooking).catch((err) => setError(err.message));
  }, [bookingId]);

  if (error) return <div className="page"><p className="alert alert--error">{error}</p></div>;
  if (!booking) return <div className="page"><p>Loading confirmation…</p></div>;

  return (
    <div className="page page--centered">
      <h1 className="page-title">You're booked!</h1>
      <p className="page-subtitle">Save your reference: <strong>{booking.bookingReference}</strong></p>

      <BoardingPass booking={booking} />

      <Link to="/manage" className="btn btn--ghost" style={{ marginTop: '2rem' }}>View my trips</Link>
    </div>
  );
}
