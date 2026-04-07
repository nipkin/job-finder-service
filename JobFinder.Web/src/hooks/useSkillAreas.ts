import { useCallback, useEffect, useState } from 'react';
import { type SkillArea, userProfileService } from '../services/userProfileService';

export function useSkillAreas() {
  const [skillAreas, setSkillAreas] = useState<SkillArea[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const load = useCallback(async () => {
    try {
      const profile = await userProfileService.getProfile();
      setSkillAreas(profile.userSkills);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load skill areas');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { load(); }, [load]);

  async function createArea(name: string, skillWeight: number) {
    const area = await userProfileService.createSkillArea(name, skillWeight);
    setSkillAreas(prev => [...prev, area]);
  }

  async function updateArea(areaId: string, name: string, skillWeight: number) {
    const area = await userProfileService.updateSkillArea(areaId, name, skillWeight);
    setSkillAreas(prev => prev.map(a => a.id === areaId ? { ...a, name: area.name, skillWeight: area.skillWeight } : a));
  }

  async function deleteArea(areaId: string) {
    await userProfileService.deleteSkillArea(areaId);
    setSkillAreas(prev => prev.filter(a => a.id !== areaId));
  }

  async function addSkill(areaId: string, name: string) {
    const skill = await userProfileService.addSkill(areaId, name);
    setSkillAreas(prev =>
      prev.map(a => a.id === areaId ? { ...a, skills: [...a.skills, skill] } : a)
    );
  }

  async function updateSkill(areaId: string, skillId: string, name: string) {
    const skill = await userProfileService.updateSkill(skillId, name);
    setSkillAreas(prev =>
      prev.map(a => a.id === areaId
        ? { ...a, skills: a.skills.map(s => s.id === skillId ? skill : s) }
        : a
      )
    );
  }

  async function removeSkill(areaId: string, skillId: string) {
    await userProfileService.removeSkill(skillId);
    setSkillAreas(prev =>
      prev.map(a => a.id === areaId
        ? { ...a, skills: a.skills.filter(s => s.id !== skillId) }
        : a
      )
    );
  }

  return { skillAreas, loading, error, createArea, updateArea, deleteArea, addSkill, updateSkill, removeSkill };
}
