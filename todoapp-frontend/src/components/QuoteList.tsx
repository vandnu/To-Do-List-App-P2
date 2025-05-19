import React, { useState, useEffect } from "react";
import { getQuotes, deleteQuote, updateQuote } from "../api/quotes";
import { useAuthStore } from "../store/authStore";

const QuoteList: React.FC = () => {
  const [quotes, setQuotes] = useState<any[]>([]);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editText, setEditText] = useState("");
  const [editAuthor, setEditAuthor] = useState("");
  const { token, role } = useAuthStore();

  const fetchQuotes = async () => {
    try {
      const data = await getQuotes();
      setQuotes(data);
    } catch (err) {
      console.error("Failed to fetch quotes", err);
    }
  };

  useEffect(() => {
    fetchQuotes();
  }, []);

  const handleDelete = async (id: number) => {
    if (!token) return;
    try {
      await deleteQuote(id, token);
      fetchQuotes();
    } catch (err) {
      console.error("Failed to delete quote", err);
    }
  };

  const handleEdit = (quote: any) => {
    setEditingId(quote.id);
    setEditText(quote.text);
    setEditAuthor(quote.author);
  };

  const handleUpdate = async (id: number) => {
    if (!token) return;
    try {
      await updateQuote(id, editText, editAuthor, token);
      setEditingId(null);
      fetchQuotes();
    } catch (err) {
      console.error("Failed to update quote", err);
    }
  };

  return (
    <div>
      <h2>Quotes</h2>
      <ul>
        {quotes.map((quote) => (
          <li key={quote.id}>
            {editingId === quote.id ? (
              <>
                <input
                  type="text"
                  value={editText}
                  onChange={(e) => setEditText(e.target.value)}
                />
                <input
                  type="text"
                  value={editAuthor}
                  onChange={(e) => setEditAuthor(e.target.value)}
                />
                <button onClick={() => handleUpdate(quote.id)}>Save</button>
                <button onClick={() => setEditingId(null)}>Cancel</button>
              </>
            ) : (
              <>
                {quote.text} - {quote.author}
                {role === "Admin" && (
                  <>
                    <button onClick={() => handleEdit(quote)}>Edit</button>
                    <button onClick={() => handleDelete(quote.id)}>Delete</button>
                  </>
                )}
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default QuoteList;