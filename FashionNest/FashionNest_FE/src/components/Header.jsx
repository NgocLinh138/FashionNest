import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHome, faShoppingCart, faUser } from '@fortawesome/free-solid-svg-icons';

const Header = () => {
  const navigate = useNavigate();
  const token = localStorage.getItem('authToken');
  const role = localStorage.getItem('userRole');
  const [cartItemCount, setCartItemCount] = useState(0);

  const getCartItemCount = () => {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    return cart.reduce((total, item) => total + item.quantity, 0);
  };

  useEffect(() => {
    setCartItemCount(getCartItemCount());
  }, []);

  useEffect(() => {
    const handleStorageChange = () => {
      setCartItemCount(getCartItemCount());
    };

    window.addEventListener('storage', handleStorageChange);

    return () => {
      window.removeEventListener('storage', handleStorageChange);
    };
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userRole');
    navigate('/login');
  };

  return (
    <header className="flex justify-between items-center p-4 bg-white border-b-4 border-pink-400 fixed top-0 left-0 right-0 z-50 shadow-lg">
      <div className="flex items-center">
        <Link
          to={role === 'Admin' ? '/admin/dashboard' : '/'}
          className="text-lg font-medium hover:text-pink-700 transition-colors"
        >
          <FontAwesomeIcon icon={faHome} size="lg" className="cursor-pointer" />
        </Link>
      </div>

      <nav className="flex gap-6 items-center">
        <Link to="/profile" className="text-lg font-medium hover:text-pink-700 transition-colors">
          <FontAwesomeIcon icon={faUser} size="lg" className="mr-2" />
        </Link>

        {(!token || role !== 'Admin') && (
          <>
            <Link to="/cart" className="text-lg font-medium hover:text-pink-700 transition-colors relative">
              <FontAwesomeIcon icon={faShoppingCart} size="lg" className="mr-2" />
              {cartItemCount > 0 && (
                <span className="absolute top-0 right-0 bg-red-500 text-white text-xs rounded-full px-1.5 py-0.5 transform translate-x-1 -translate-y-1 min-w-[5px] text-center">
                  {cartItemCount > 99 ? '99+' : cartItemCount}
                </span>
              )}
            </Link>
          </>
        )}

        {token ? (
          <>
            <span onClick={handleLogout} className="text-lg font-medium hover:text-pink-500 cursor-pointer transition-colors">Đăng xuất</span>
          </>
        ) : (
          <>
            <Link to="/login" className="text-lg font-medium hover:text-pink-700 transition-colors">Đăng nhập</Link>
            <Link to="/register" className="text-lg font-medium hover:text-pink-700 transition-colors">Đăng ký</Link>
          </>
        )}
      </nav>
    </header>
  );
};

export default Header;
