import React, { useState } from 'react';
import FlightSearchForm from '../components/FlightSearchForm';
import FlightList from '../components/FlightList';
import { api } from '../api/client';

export default function HomePage() {
  const [flights, setFlights] = useState([]);
  const [searched, setSearched] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [lastQuery, setLastQuery] = useState(null);

  const handleSearch = async (form) => {
    setLoading(true);
    setError('');
    try {
      const results = await api.searchFlights(form);
      setFlights(results);
      setLastQuery(form);
      setSearched(true);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="page">
      <section className="hero">
        <h1 className="hero__title">Fly further, for less.</h1>
        <p className="hero__subtitle">Search live fares across our network and lock in your seat in under a minute.</p>
        <FlightSearchForm onSearch={handleSearch} loading={loading} />
      </section>

      {error && <p className="alert alert--error">{error}</p>}

      {searched && (
        <section className="results">
          <h2 className="results__heading">
            {flights.length} flight{flights.length !== 1 ? 's' : ''} found
          </h2>
          <FlightList
            flights={flights}
            seatClass={lastQuery.seatClass}
            passengers={lastQuery.passengers}
          />
        </section>
      )}
    </div>
  );
}
