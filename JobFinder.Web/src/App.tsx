import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Layout from './layout/Layout';
import Start from './pages/Start';
import Login from './pages/Login';
import MyPage from './pages/MyPage';

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route element={<Layout />}>
                    <Route path="/" element={<Start />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/my-page" element={<MyPage />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default App;
