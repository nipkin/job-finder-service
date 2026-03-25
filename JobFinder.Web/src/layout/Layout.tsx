import { Outlet } from 'react-router-dom';
import TopNav from './TopNav';

export default function Layout() {
  return (
    <div>
      <TopNav />
      <main>
        <Outlet />
      </main>
    </div>
  );
}
