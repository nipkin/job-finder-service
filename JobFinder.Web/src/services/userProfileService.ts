const BASE_URL = '/api/userprofile';

export interface Skill {
  id: string;
  name: string;
}

export interface SkillArea {
  id: string;
  name: string;
  skills: Skill[];
  skillWeight: number;
}

export interface UserProfile {
  id: string;
  userName: string;
  userSkills: SkillArea[];
}

async function request<T>(url: string, options?: RequestInit): Promise<T> {
  const response = await fetch(url, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });

  if (!response.ok) {
    const error = await response.json().catch(() => ({}));
    throw new Error(error.message ?? 'Request failed');
  }

  if (response.status === 204) return undefined as T;
  return response.json();
}

function getProfile(): Promise<UserProfile> {
  return request(BASE_URL);
}

function createSkillArea(name: string, skillWeight: number): Promise<SkillArea> {
  return request(`${BASE_URL}/skillareas`, {
    method: 'POST',
    body: JSON.stringify({ name, skills: [], skillWeight }),
  });
}

function deleteSkillArea(areaId: string): Promise<void> {
  return request(`${BASE_URL}/skillareas/${areaId}`, { method: 'DELETE' });
}

function addSkill(areaId: string, name: string): Promise<Skill> {
  return request(`${BASE_URL}/skillareas/${areaId}/skills`, {
    method: 'POST',
    body: JSON.stringify({ name }),
  });
}

function updateSkill(skillId: string, name: string): Promise<Skill> {
  return request(`${BASE_URL}/skills/${skillId}`, {
    method: 'PUT',
    body: JSON.stringify({ name }),
  });
}

function removeSkill(skillId: string): Promise<void> {
  return request(`${BASE_URL}/skills/${skillId}`, { method: 'DELETE' });
}

export const userProfileService = {
  getProfile,
  createSkillArea,
  deleteSkillArea,
  addSkill,
  updateSkill,
  removeSkill,
};
