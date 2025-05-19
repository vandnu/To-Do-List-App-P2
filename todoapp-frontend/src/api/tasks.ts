import axios from "axios";

const API_URL = "https://localhost:7100/api/ToDoTasks";

export const createTask = async (title: string, description: string | null, token: string) => {
  const response = await axios.post(
    API_URL,
    { title, description, isCompleted: false },
    { headers: { Authorization: `Bearer ${token}` } }
  );
  return response.data;
};

export const getMyTasks = async (token: string) => {
  const response = await axios.get(`${API_URL}/my-tasks`, {
    headers: { Authorization: `Bearer ${token}` } },
  );
  return response.data; // Liste af tasks
};