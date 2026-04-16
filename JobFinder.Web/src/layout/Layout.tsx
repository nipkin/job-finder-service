import { Outlet } from 'react-router-dom';
import TopNav from './TopNav';

export default function Layout() {
  return (
    <div className="min-h-screen bg-slate-950 text-slate-100 flex flex-col">
      <TopNav />
      <main className="flex-1 px-4 py-8 max-w-4xl mx-auto w-full">
        <Outlet />
      </main>
    </div>
  );
}
