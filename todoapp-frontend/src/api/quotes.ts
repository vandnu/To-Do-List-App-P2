import axios from "axios";

const API_URL = "https://localhost:7100/api/Quotes";

export const getQuotes = async () => {
  const response = await axios.get(API_URL);
  return response.data; // Liste af quotes
};

// Admin: Slet quote
export const deleteQuote = async (id: number, token: string) => {
  await axios.delete(`${API_URL}/${id}`, {
    headers: { Authorization: `Bearer ${token}` } },
  );
};

// Admin: Opdater quote
export const updateQuote = async (id: number, text: string, author: string, token: string) => {
  await axios.put(
    `${API_URL}/${id}`,
    { id, text, author },
    { headers: { Authorization: `Bearer ${token}` } }
  );
};