import React, { createContext, useContext, useState } from 'react';
import { api, saveSession, getSession, clearSession } from '../api/client';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(getSession());

  const login = async (email, password) => {
    const response = await api.login({ email, password });
    saveSession(response);
    setUser(getSession());
    return response;
  };

  const register = async (payload) => {
    const response = await api.register(payload);
    saveSession(response);
    setUser(getSession());
    return response;
  };

  const logout = () => {
    clearSession();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}
