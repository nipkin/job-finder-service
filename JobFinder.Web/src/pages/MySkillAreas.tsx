import { useState } from 'react';
import { useSkillAreas } from '../hooks';

export default function MySkillAreas() {
  const { skillAreas, loading, error, createArea, deleteArea, addSkill, updateSkill, removeSkill } = useSkillAreas();

  const [newAreaName, setNewAreaName] = useState('');
  const [newAreaWeight, setNewAreaWeight] = useState(3);
  const [newSkillInputs, setNewSkillInputs] = useState<Record<string, string>>({});
  const [editingSkill, setEditingSkill] = useState<{ areaId: string; skillId: string; value: string } | null>(null);
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

  if (loading) return <p>Loading...</p>;
  if (error) return <p style={{ color: 'red' }}>{error}</p>;

  return (
    <div style={{ maxWidth: 600, margin: '0 auto', padding: '1rem' }}>
      <h1>My Skill Areas</h1>

      <form onSubmit={handleCreateArea} style={{ display: 'flex', gap: '0.5rem', marginBottom: '2rem', alignItems: 'flex-end' }}>
        <div style={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
          <label htmlFor="areaName">New skill area</label>
          <input
            id="areaName"
            type="text"
            placeholder="e.g. Backend"
            value={newAreaName}
            onChange={e => setNewAreaName(e.target.value)}
          />
        </div>
        <div style={{ display: 'flex', flexDirection: 'column' }}>
          <label htmlFor="areaWeight">Weight (1–5)</label>
          <input
            id="areaWeight"
            type="number"
            min={1}
            max={5}
            value={newAreaWeight}
            onChange={e => setNewAreaWeight(Number(e.target.value))}
            style={{ width: 60 }}
          />
        </div>
        <button type="submit">Add</button>
      </form>

      {formError && <p style={{ color: 'red' }}>{formError}</p>}

      {skillAreas.length === 0 && <p>No skill areas yet.</p>}

      {skillAreas.map(area => (
        <div key={area.id} style={{ border: '1px solid #444', borderRadius: 8, padding: '1rem', marginBottom: '1rem' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '0.75rem' }}>
            <span style={{ fontWeight: 600 }}>{area.name} <span style={{ fontSize: '0.85em', opacity: 0.6 }}>weight {area.skillWeight}</span></span>
            <button onClick={() => deleteArea(area.id)} style={{ color: 'red' }}>Delete area</button>
          </div>

          <ul style={{ listStyle: 'none', padding: 0, margin: '0 0 0.75rem 0' }}>
            {area.skills.map(skill => (
              <li key={skill.id} style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginBottom: '0.4rem' }}>
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
                      style={{ flex: 1 }}
                    />
                    <button onClick={handleUpdateSkill}>Save</button>
                    <button onClick={() => setEditingSkill(null)}>Cancel</button>
                  </>
                ) : (
                  <>
                    <span
                      style={{ flex: 1, cursor: 'pointer' }}
                      onClick={() => setEditingSkill({ areaId: area.id, skillId: skill.id, value: skill.name })}
                      title="Click to edit"
                    >
                      {skill.name}
                    </span>
                    <button onClick={() => removeSkill(area.id, skill.id)} style={{ color: 'red' }}>✕</button>
                  </>
                )}
              </li>
            ))}
          </ul>

          <div style={{ display: 'flex', gap: '0.5rem' }}>
            <input
              type="text"
              placeholder="Add skill..."
              value={newSkillInputs[area.id] ?? ''}
              onChange={e => setNewSkillInputs(prev => ({ ...prev, [area.id]: e.target.value }))}
              onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); handleAddSkill(area.id); } }}
              style={{ flex: 1 }}
            />
            <button onClick={() => handleAddSkill(area.id)}>Add</button>
          </div>
        </div>
      ))}
    </div>
  );
}