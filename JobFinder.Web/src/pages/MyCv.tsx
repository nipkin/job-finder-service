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
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-3xl font-bold text-white">My CV</h1>
        <p className="text-slate-400 mt-1">Paste your CV below. The AI uses this when scoring job matches.</p>
      </div>

      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <textarea
          value={displayText}
          onChange={e => { setCvText(e.target.value); setSaved(false); }}
          rows={20}
          placeholder="Paste your CV here..."
          className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white placeholder:text-slate-500 focus:outline-none focus:ring-2 focus:ring-indigo-500 resize-y font-mono text-sm leading-relaxed"
        />
        <div className="flex items-center gap-4">
          <button
            type="submit"
            disabled={loading || !displayText.trim()}
            className="bg-indigo-600 hover:bg-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed text-white px-5 py-2 rounded-lg font-medium transition-colors"
          >
            {loading ? 'Saving...' : 'Save CV'}
          </button>
          {saved && <span className="text-emerald-400 text-sm">Saved successfully.</span>}
          {error && <span className="text-red-400 text-sm">{error}</span>}
        </div>
      </form>
    </div>
  );
}
