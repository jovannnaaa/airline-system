import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [form, setForm] = useState({ firstName: '', lastName: '', email: '', password: '' });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (field) => (e) => setForm({ ...form, [field]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      await register(form);
      navigate('/');
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="page page--narrow">
      <h1 className="page-title">Create your account</h1>
      <form className="auth-form" onSubmit={handleSubmit}>
        <div className="auth-form__row">
          <label className="field">
            <span>First name</span>
            <input required value={form.firstName} onChange={handleChange('firstName')} />
          </label>
          <label className="field">
            <span>Last name</span>
            <input required value={form.lastName} onChange={handleChange('lastName')} />
          </label>
        </div>
        <label className="field">
          <span>Email</span>
          <input type="email" required value={form.email} onChange={handleChange('email')} />
        </label>
        <label className="field">
          <span>Password</span>
          <input type="password" required minLength={6} value={form.password} onChange={handleChange('password')} />
        </label>
        {error && <p className="alert alert--error">{error}</p>}
        <button className="btn btn--primary btn--block" disabled={loading}>
          {loading ? 'Creating account…' : 'Create account'}
        </button>
      </form>
      <p className="auth-form__footer">
        Already have an account? <Link to="/login">Sign in</Link>
      </p>
    </div>
  );
}
