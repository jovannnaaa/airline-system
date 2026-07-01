import React, { useEffect, useState } from 'react';
import { api } from '../api/client';

export default function FlightSearchForm({ onSearch, loading }) {
  const [airports, setAirports] = useState([]);
  const [form, setForm] = useState({
    departureCode: '',
    arrivalCode: '',
    date: '',
    seatClass: 'Economy',
    passengers: 1,
  });

  useEffect(() => {
    api.getAirports().then(setAirports).catch(() => setAirports([]));
  }, []);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSearch(form);
  };

  return (
    <form className="search-panel" onSubmit={handleSubmit}>
      <div className="search-panel__row">
        <label className="field">
          <span>From</span>
          <select name="departureCode" value={form.departureCode} onChange={handleChange} required>
            <option value="">Select airport</option>
            {airports.map((a) => (
              <option key={a.id} value={a.code}>{a.city} ({a.code})</option>
            ))}
          </select>
        </label>

        <div className="search-panel__swap" aria-hidden="true">&#8646;</div>

        <label className="field">
          <span>To</span>
          <select name="arrivalCode" value={form.arrivalCode} onChange={handleChange} required>
            <option value="">Select airport</option>
            {airports.map((a) => (
              <option key={a.id} value={a.code}>{a.city} ({a.code})</option>
            ))}
          </select>
        </label>
      </div>

      <div className="search-panel__row">
        <label className="field">
          <span>Departure date</span>
          <input type="date" name="date" value={form.date} onChange={handleChange} />
        </label>

        <label className="field">
          <span>Cabin</span>
          <select name="seatClass" value={form.seatClass} onChange={handleChange}>
            <option value="Economy">Economy</option>
            <option value="Business">Business</option>
            <option value="FirstClass">First class</option>
          </select>
        </label>

        <label className="field field--narrow">
          <span>Passengers</span>
          <input
            type="number"
            name="passengers"
            min="1"
            max="9"
            value={form.passengers}
            onChange={handleChange}
          />
        </label>

        <button type="submit" className="btn btn--primary btn--block" disabled={loading}>
          {loading ? 'Searching…' : 'Search flights'}
        </button>
      </div>
    </form>
  );
}
