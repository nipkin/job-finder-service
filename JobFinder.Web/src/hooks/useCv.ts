import { useCallback, useEffect, useState } from 'react';
import { userProfileService, type UserCv } from '../services/userProfileService';

export function useCv() {
    const [cv, setCv] = useState<UserCv | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const load = useCallback(async () => {
        try {
            const data = await userProfileService.getCvText();
            setCv(data);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Failed to load CV');
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => { load(); }, [load]);

    async function updateCv(cvText: string) {
        try {
            setLoading(true);
            const updated = await userProfileService.updateCvText(cvText);
            setCv(updated);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Failed to update CV');
        } finally {
            setLoading(false);
        }
    }

    return { cv, loading, error, updateCv };
}