import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <header className="navbar">
      <Link to="/" className="navbar__brand">
        <span className="navbar__mark">&#9992;</span>
        Skyward Air
      </Link>

      <nav className="navbar__links">
        <Link to="/">Search flights</Link>
        <Link to="/manage">My trips</Link>
      </nav>

      <div className="navbar__auth">
        {user ? (
          <>
            <span className="navbar__greeting">Hi, {user.firstName}</span>
            <button className="btn btn--ghost" onClick={handleLogout}>Sign out</button>
          </>
        ) : (
          <>
            <Link to="/login" className="btn btn--ghost">Sign in</Link>
            <Link to="/register" className="btn btn--primary">Create account</Link>
          </>
        )}
      </div>
    </header>
  );
}
