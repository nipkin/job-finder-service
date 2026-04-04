import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { AuthProvider } from './context/AuthProvider';
import Layout from './layout/Layout';
import ProtectedRoute from './components/ProtectedRoute';
import Start from './pages/Start';
import Login from './pages/Login';
import Register from './pages/Register';
import MyPage from './pages/MyPage';
import MySkillAreas from './pages/MySkillAreas';

function App() {
    return (
        <BrowserRouter>
            <AuthProvider>
                <Routes>
                    <Route element={<Layout />}>
                        <Route path="/" element={<Start />} />
                        <Route path="/login" element={<Login />} />
                        <Route path="/register" element={<Register />} />
                        <Route element={<ProtectedRoute />}>
                            <Route path="/my-page" element={<MyPage />} />
                            <Route path="/my-skill-areas" element={<MySkillAreas />} />
                        </Route>
                    </Route>
                </Routes>
            </AuthProvider>
        </BrowserRouter>
    );
}

export default App;
