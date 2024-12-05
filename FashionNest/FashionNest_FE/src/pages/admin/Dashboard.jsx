import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faBox } from '@fortawesome/free-solid-svg-icons';
import ManageUser from './ManageUser';
import ManageProduct from './ManageProduct';
import ManageCategory from './ManageCategory';
import ManageOrder from './ManageOrder';

const Dashboard = () => {
  const [selectedSection, setSelectedSection] = useState('user');

  const handleSidebarClick = (section) => {
    setSelectedSection(section);
  };

  return (
    <div className="flex min-h-screen bg-gray-100">
      {/* Sidebar */}
      <div className="w-64 bg-gray-800 text-white p-6 pt-24">
        <h3 className="text-3xl font-semibold mb-8 text-center text-white">Dashboard</h3>

        <ul className="space-y-6">
          <li
            onClick={() => handleSidebarClick('user')}
            className={`cursor-pointer flex items-center space-x-4 p-3 rounded-lg hover:bg-gray-700 transition duration-300 ease-in-out ${selectedSection === 'user' ? 'bg-gray-700' : ''}`}
          >
            <FontAwesomeIcon icon={faUser} className="text-xl" />
            <span>Manage User</span>
          </li>
          <li
            onClick={() => handleSidebarClick('order')}
            className={`cursor-pointer flex items-center space-x-4 p-3 rounded-lg hover:bg-gray-700 transition duration-300 ease-in-out ${selectedSection === 'order' ? 'bg-gray-700' : ''}`}
          >
            <FontAwesomeIcon icon={faBox} className="text-xl" />
            <span>Manage Order</span>
          </li>
          <li
            onClick={() => handleSidebarClick('product')}
            className={`cursor-pointer flex items-center space-x-4 p-3 rounded-lg hover:bg-gray-700 transition duration-300 ease-in-out ${selectedSection === 'product' ? 'bg-gray-700' : ''}`}
          >
            <FontAwesomeIcon icon={faBox} className="text-xl" />
            <span>Manage Product</span>
          </li>
          <li
            onClick={() => handleSidebarClick('category')}
            className={`cursor-pointer flex items-center space-x-4 p-3 rounded-lg hover:bg-gray-700 transition duration-300 ease-in-out ${selectedSection === 'category' ? 'bg-gray-700' : ''}`}
          >
            <FontAwesomeIcon icon={faBox} className="text-xl" />
            <span>Manage Category</span>
          </li>
        </ul>
      </div>

      {/* Main Content */}
      <div className="flex-1 p-6 mt-24">
        {selectedSection === 'user' && <ManageUser />}
        {selectedSection === 'order' && <ManageOrder />}
        {selectedSection === 'product' && <ManageProduct />}
        {selectedSection === 'category' && <ManageCategory />}
      </div>
    </div>
  );
};

export default Dashboard;
