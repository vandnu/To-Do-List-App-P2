import axios from "axios";

const API_URL = "https://localhost:7100/api/Auth";

// Login
export const login = async (username: string, password: string): Promise<string> => {
  const response = await axios.post(`${API_URL}/login`, { username, password });
  const token = response.data.token;

  // Dekodér JWT-token for at hente userId (simpel dekoding uden bibliotek)
  const payload = token.split(".")[1];
  const decoded = JSON.parse(atob(payload));
  const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
  if (userId) {
    localStorage.setItem("userId", userId); // Gem userId i localStorage
  }

  return token; // Returner token
};

// Registrering
export const register = async (username: string, password: string, email: string) => {
  const response = await axios.post(`${API_URL}/register`, { username, password, email });
  return response.data; // Forventer "User created successfully"
};