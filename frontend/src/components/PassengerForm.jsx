import React from 'react';

export default function PassengerForm({ index, passenger, onChange }) {
  const handleField = (field) => (e) => {
    onChange(index, { ...passenger, [field]: e.target.value });
  };

  return (
    <fieldset className="passenger-form">
      <legend>Passenger {index + 1}</legend>

      <div className="passenger-form__row">
        <label className="field">
          <span>First name</span>
          <input value={passenger.firstName} onChange={handleField('firstName')} required />
        </label>
        <label className="field">
          <span>Last name</span>
          <input value={passenger.lastName} onChange={handleField('lastName')} required />
        </label>
      </div>

      <div className="passenger-form__row">
        <label className="field">
          <span>Passport number</span>
          <input value={passenger.passportNumber} onChange={handleField('passportNumber')} required />
        </label>
        <label className="field">
          <span>Date of birth</span>
          <input type="date" value={passenger.dateOfBirth} onChange={handleField('dateOfBirth')} required />
        </label>
      </div>

      {index === 0 && (
        <div className="passenger-form__row">
          <label className="field">
            <span>Email</span>
            <input type="email" value={passenger.email} onChange={handleField('email')} />
          </label>
          <label className="field">
            <span>Phone</span>
            <input value={passenger.phone} onChange={handleField('phone')} />
          </label>
        </div>
      )}
    </fieldset>
  );
}
