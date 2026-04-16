import { useState } from 'react';
import { useCv } from '../hooks/useCv';

export default function MyCv() {
    const { cv, loading, error, updateCv } = useCv();
    const [cvText, setCvText] = useState<string | null>(null);
    const [saved, setSaved] = useState(false);

    const displayText = cvText ?? cv?.cvText ?? '';

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setSaved(false);
    await updateCv(displayText);
    setSaved(true);
  }

  return (
    <div style={{ maxWidth: 800, margin: '0 auto', padding: '1rem' }}>
      <h1>My CV</h1>
      <p>Paste your CV text below. This will be used by the AI assistant when matching job postings.</p>

      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
        <textarea
          value={displayText}
          onChange={e => { setCvText(e.target.value); setSaved(false); }}
          rows={20}
          placeholder="Paste your CV here..."
          style={{ width: '100%', resize: 'vertical', padding: '0.75rem', boxSizing: 'border-box' }}
        />
        <div>
           <button type="submit" disabled={loading || !displayText.trim()}>
            {loading ? 'Saving...' : 'Save CV'}
          </button>
        </div>
        {saved && <p style={{ color: 'green' }}>CV saved successfully.</p>}
        {error && <p style={{ color: 'red' }}>{error}</p>}
      </form>
    </div>
  );
}