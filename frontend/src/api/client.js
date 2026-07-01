const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

function getToken() {
  return localStorage.getItem('skyward_token');
}

async function request(path, { method = 'GET', body, auth = false } = {}) {
  const headers = { 'Content-Type': 'application/json' };
  if (auth) {
    const token = getToken();
    if (token) headers['Authorization'] = `Bearer ${token}`;
  }

  const res = await fetch(`${API_BASE_URL}${path}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  });

  if (!res.ok) {
    let message = `Request failed (${res.status})`;
    try {
      const data = await res.json();
      message = data.title || data.message || (typeof data === 'string' ? data : message);
    } catch {
      const text = await res.text().catch(() => '');
      if (text) message = text;
    }
    throw new Error(message);
  }

  if (res.status === 204) return null;
  return res.json();
}

export const api = {
  // Airports
  getAirports: () => request('/airports'),

  // Flights
  searchFlights: (params) => {
    const query = new URLSearchParams();
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') query.append(key, value);
    });
    return request(`/flights/search?${query.toString()}`);
  },
  getFlight: (id) => request(`/flights/${id}`),

  // Bookings
  createBooking: (payload) => request('/bookings', { method: 'POST', body: payload }),
  getBooking: (id) => request(`/bookings/${id}`),
  getBookingByReference: (reference) => request(`/bookings/reference/${reference}`),
  getMyBookings: (userId) => request(`/bookings?userId=${userId}`),
  cancelBooking: (id) => request(`/bookings/${id}/cancel`, { method: 'PUT' }),

  // Auth
  register: (payload) => request('/auth/register', { method: 'POST', body: payload }),
  login: (payload) => request('/auth/login', { method: 'POST', body: payload }),
};

export function saveSession(authResponse) {
  localStorage.setItem('skyward_token', authResponse.token);
  localStorage.setItem('skyward_user', JSON.stringify({
    userId: authResponse.userId,
    email: authResponse.email,
    firstName: authResponse.firstName,
    lastName: authResponse.lastName,
    role: authResponse.role,
  }));
}

export function getSession() {
  const raw = localStorage.getItem('skyward_user');
  return raw ? JSON.parse(raw) : null;
}

export function clearSession() {
  localStorage.removeItem('skyward_token');
  localStorage.removeItem('skyward_user');
}
