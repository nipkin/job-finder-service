import { useState } from 'react';
import { useSkillAreas } from '../hooks';

interface EditingArea {
  areaId: string;
  name: string;
  skillWeight: number;
}

export default function MySkillAreas() {
  const { skillAreas, loading, error, createArea, updateArea, deleteArea, addSkill, updateSkill, removeSkill } = useSkillAreas();

  const [newAreaName, setNewAreaName] = useState('');
  const [newAreaWeight, setNewAreaWeight] = useState(3);
  const [newSkillInputs, setNewSkillInputs] = useState<Record<string, string>>({});
  const [editingSkill, setEditingSkill] = useState<{ areaId: string; skillId: string; value: string } | null>(null);
  const [editingArea, setEditingArea] = useState<EditingArea | null>(null);
  const [formError, setFormError] = useState<string | null>(null);

  async function handleCreateArea(e: React.FormEvent) {
    e.preventDefault();
    if (!newAreaName.trim()) return;
    setFormError(null);
    try {
      await createArea(newAreaName.trim(), newAreaWeight);
      setNewAreaName('');
      setNewAreaWeight(3);
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Failed to create skill area');
    }
  }

  async function handleUpdateArea() {
    if (!editingArea) return;
    setFormError(null);
    try {
      await updateArea(editingArea.areaId, editingArea.name.trim(), editingArea.skillWeight);
      setEditingArea(null);
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Failed to update skill area');
    }
  }

  async function handleAddSkill(areaId: string) {
    const name = newSkillInputs[areaId]?.trim();
    if (!name) return;
    setFormError(null);
    try {
      await addSkill(areaId, name);
      setNewSkillInputs(prev => ({ ...prev, [areaId]: '' }));
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Failed to add skill');
    }
  }

  async function handleUpdateSkill() {
    if (!editingSkill) return;
    const { areaId, skillId, value } = editingSkill;
    if (!value.trim()) return;
    setFormError(null);
    try {
      await updateSkill(areaId, skillId, value.trim());
      setEditingSkill(null);
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Failed to update skill');
    }
  }

  if (loading) return <p className="text-slate-400">Loading...</p>;
  if (error) return <p className="text-red-400">{error}</p>;

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-3xl font-bold text-white">My Skill Areas</h1>
        <p className="text-slate-400 mt-1">Define skill areas and set their weight for AI matching.</p>
      </div>

      <form onSubmit={handleCreateArea} className="bg-slate-900 border border-slate-800 rounded-xl p-4 flex gap-3 items-end">
        <div className="flex flex-col gap-1.5 flex-1">
          <label htmlFor="areaName" className="text-sm text-slate-400">New skill area</label>
          <input
            id="areaName"
            type="text"
            placeholder="e.g. Backend"
            value={newAreaName}
            onChange={e => setNewAreaName(e.target.value)}
            className="bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white placeholder:text-slate-500 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>
        <div className="flex flex-col gap-1.5">
          <label htmlFor="areaWeight" className="text-sm text-slate-400">Weight (1–5)</label>
          <input
            id="areaWeight"
            type="number"
            min={1}
            max={5}
            value={newAreaWeight}
            onChange={e => setNewAreaWeight(Number(e.target.value))}
            className="bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white w-20 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>
        <button
          type="submit"
          className="bg-indigo-600 hover:bg-indigo-500 text-white px-4 py-2 rounded-lg font-medium transition-colors"
        >
          Add
        </button>
      </form>

      {formError && <p className="text-red-400 text-sm">{formError}</p>}
      {skillAreas.length === 0 && <p className="text-slate-500">No skill areas yet.</p>}

      <div className="flex flex-col gap-4">
        {skillAreas.map(area => (
          <div key={area.id} className="bg-slate-900 border border-slate-800 rounded-xl p-5 flex flex-col gap-4">
            <div className="flex justify-between items-center">
              {editingArea?.areaId === area.id ? (
                <div className="flex gap-2 items-center flex-1">
                  <input
                    autoFocus
                    value={editingArea.name}
                    onChange={e => setEditingArea({ ...editingArea, name: e.target.value })}
                    onKeyDown={e => { if (e.key === 'Escape') setEditingArea(null); }}
                    className="bg-slate-800 border border-slate-700 rounded-lg px-3 py-1.5 text-white flex-1 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                  />
                  <input
                    type="number"
                    min={1}
                    max={5}
                    value={editingArea.skillWeight}
                    onChange={e => setEditingArea({ ...editingArea, skillWeight: Number(e.target.value) })}
                    className="bg-slate-800 border border-slate-700 rounded-lg px-3 py-1.5 text-white w-16 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                  />
                  <button onClick={handleUpdateArea} className="bg-indigo-600 hover:bg-indigo-500 text-white px-3 py-1.5 rounded-lg text-sm transition-colors">Save</button>
                  <button onClick={() => setEditingArea(null)} className="text-slate-400 hover:text-white px-3 py-1.5 rounded-lg text-sm transition-colors">Cancel</button>
                </div>
              ) : (
                <>
                  <button
                    onClick={() => setEditingArea({ areaId: area.id, name: area.name, skillWeight: area.skillWeight })}
                    className="text-white font-semibold hover:text-indigo-400 transition-colors text-left bg-transparent border-0 p-0 cursor-pointer"
                  >
                    {area.name}
                    <span className="text-slate-500 font-normal text-sm ml-2">weight {area.skillWeight}</span>
                  </button>
                  <button
                    onClick={() => deleteArea(area.id)}
                    className="text-slate-500 hover:text-red-400 text-sm transition-colors"
                  >
                    Delete
                  </button>
                </>
              )}
            </div>

            <ul className="flex flex-col gap-2">
              {area.skills.map(skill => (
                <li key={skill.id} className="flex items-center gap-2">
                  {editingSkill?.skillId === skill.id ? (
                    <>
                      <input
                        autoFocus
                        value={editingSkill.value}
                        onChange={e => setEditingSkill({ ...editingSkill, value: e.target.value })}
                        onKeyDown={e => {
                          if (e.key === 'Enter') handleUpdateSkill();
                          if (e.key === 'Escape') setEditingSkill(null);
                        }}
                        className="bg-slate-800 border border-slate-700 rounded-lg px-3 py-1.5 text-white flex-1 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                      />
                      <button onClick={handleUpdateSkill} className="bg-indigo-600 hover:bg-indigo-500 text-white px-3 py-1.5 rounded-lg text-sm transition-colors">Save</button>
                      <button onClick={() => setEditingSkill(null)} className="text-slate-400 hover:text-white text-sm transition-colors">Cancel</button>
                    </>
                  ) : (
                    <>
                      <span
                        onClick={() => setEditingSkill({ areaId: area.id, skillId: skill.id, value: skill.name })}
                        className="flex-1 text-slate-300 text-sm cursor-pointer hover:text-white transition-colors"
                        title="Click to edit"
                      >
                        {skill.name}
                      </span>
                      <button
                        onClick={() => removeSkill(area.id, skill.id)}
                        className="text-slate-600 hover:text-red-400 text-xs transition-colors"
                      >
                        ✕
                      </button>
                    </>
                  )}
                </li>
              ))}
            </ul>

            <div className="flex gap-2">
              <input
                type="text"
                placeholder="Add skill..."
                value={newSkillInputs[area.id] ?? ''}
                onChange={e => setNewSkillInputs(prev => ({ ...prev, [area.id]: e.target.value }))}
                onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); handleAddSkill(area.id); } }}
                className="flex-1 bg-slate-800 border border-slate-700 rounded-lg px-3 py-1.5 text-white text-sm placeholder:text-slate-500 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
              <button
                onClick={() => handleAddSkill(area.id)}
                className="bg-slate-700 hover:bg-slate-600 text-white px-3 py-1.5 rounded-lg text-sm transition-colors"
              >
                Add
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
