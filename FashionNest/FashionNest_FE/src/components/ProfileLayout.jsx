import React from 'react';
import { Link, useLocation } from 'react-router-dom';

const ProfileLayout = ({ children }) => {
  const location = useLocation();

  const links = [
    { to: "/profile", label: "Hồ sơ" },
    { to: "/change-password", label: "Đổi mật khẩu" },
    { to: "/orders", label: "Đơn mua" },
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-100 via-blue-50 to-gray-50 py-12 px-6 sm:px-10 lg:px-16 mt-16 flex">
      <aside className="w-1/4 bg-white p-6 rounded-lg shadow-lg">
        <h3 className="text-2xl font-semibold text-gray-800 mb-6 border-b pb-3 border-gray-200">
          Tài khoản của tôi
        </h3>
        <ul className="space-y-4">
          {links.map((link) => (
            <li key={link.to}>
              <Link
                to={link.to}
                className={`flex items-center gap-3 text-lg p-3 rounded-lg transition-colors duration-300 ease-in-out ${location.pathname === link.to
                  ? "bg-gray-200 font-bold"
                  : "text-gray-700 hover:bg-gray-100"
                  }`}
              >
                {link.label}
              </Link>
            </li>
          ))}
        </ul>
      </aside>

      <main className="flex-1 bg-white p-10 rounded-2xl shadow-xl ml-6">
        {children}
      </main>
    </div>
  );
};

export default ProfileLayout;
