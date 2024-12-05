import React, { useState, useEffect } from 'react';
import { getAllUsers } from '../../services/userService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortUp, faSortDown, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import Swal from 'sweetalert2';
import { updateUserAPI, deleteUser } from '../../services/userService';
import DataTable from '../../components/DataTable';

const ManageUser = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState('name');
  const [sortOrderAscending, setSortOrderAscending] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [updateUser, setUpdateUser] = useState(null);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const fetchedUsers = await getAllUsers(searchTerm, sortBy, sortOrderAscending);
        setUsers(fetchedUsers);
        setLoading(false);
      } catch (error) {
        console.error('Failed to fetch user data:', error);
        setLoading(false);
      }
    };

    fetchUsers();
  }, [searchTerm, sortBy, sortOrderAscending]);

  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };

  const handleSortChange = (field) => {
    if (sortBy === field) {
      setSortOrderAscending(!sortOrderAscending);
    } else {
      setSortBy(field);
      setSortOrderAscending(true);
    }
  };

  const columns = [
    { field: 'id', label: 'ID' },
    { field: 'name', label: 'Name', sortable: true },
    { field: 'email', label: 'Email', sortable: true },
    { field: 'phoneNumber', label: 'Phone Number' },
    { field: 'address', label: 'Address' },
    {
      field: 'isActive',
      label: 'Status',
      render: (row) => (
        <span
          className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold ${row.isActive ? 'text-green-700 bg-green-100' : 'text-red-700 bg-red-100'
            }`}
        >
          {row.isActive ? 'Active' : 'Inactive'}
        </span>
      ),
    },
    {
      field: 'view',
      label: 'View',
      render: (row) => (
        <span
          onClick={() => openModal(row)}
          className="cursor-pointer"
        >
          <FontAwesomeIcon icon={faArrowRight} className="text-xl" />
        </span>
      ),
    },
  ];



  const openModal = (user) => {
    setSelectedUser(user);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setSelectedUser(null);
  };

  const openUpdateModal = (user) => {
    setUpdateUser({ ...user });
    setIsUpdateModalOpen(true);
  };

  const closeUpdateModal = () => {
    setIsUpdateModalOpen(false);
    setUpdateUser(null);
  };

  const handleUpdate = async () => {
    try {
      const updatedUserData = {
        userId: updateUser.id,
        name: updateUser.name,
        phoneNumber: updateUser.phoneNumber,
        address: updateUser.address,
      };

      await updateUserAPI(updatedUserData);
      Swal.fire({
        title: 'Thành công!',
        text: 'Chi tiết người dùng được cập nhật thành công.',
        icon: 'success',
        showConfirmButton: false,
        showCancelButton: false,
        background: '#fff',
        timer: 1000
      })
      setIsUpdateModalOpen(false);

      const fetchedUsers = await getAllUsers(searchTerm, sortBy, sortOrderAscending);
      setUsers(fetchedUsers);
    } catch (error) {
      Swal.fire('Error!', 'There was an issue updating the user.', 'error');
    }
  };

  const handleDelete = async () => {
    const result = await Swal.fire({
      title: 'Are you sure?',
      text: "This action cannot be undone!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel',
    });

    if (result.isConfirmed) {
      try {
        await deleteUser(selectedUser.id);
        setUsers(users.filter((user) => user.id !== selectedUser.id));
        closeModal();
        Swal.fire({
          title: 'Thành công!',
          text: 'Người dùng đã được xóa.',
          icon: 'success',
          showConfirmButton: false,
          showCancelButton: false,
          background: '#fff',
          timer: 1000
        })
      } catch (error) {
        Swal.fire('Error!', 'There was an issue deleting the user.', 'error');
      }
    }
  };

  if (loading) {
    return <div className="text-center py-6">Loading data...</div>;
  }

  return (
    <div className="p-6 bg-white rounded-lg shadow-md">
      <h2 className="text-2xl font-semibold mb-6">Manage Users</h2>

      <div className="mb-4">
        <input
          type="text"
          placeholder="Search users..."
          value={searchTerm}
          onChange={handleSearchChange}
          className="w-full p-2 border rounded-lg"
        />
      </div>

      <DataTable
        data={users}
        columns={columns}
        sortBy={sortBy}
        sortOrderAscending={sortOrderAscending}
        onSortChange={handleSortChange}
        onRowClick={() => { }}
      />


      {isModalOpen && selectedUser && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white p-6 rounded-lg w-96 shadow-lg transform transition-all scale-100 hover:scale-105">
            <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">User Details</h3>

            <div className="space-y-4">
              <p><strong>ID:</strong> {selectedUser.id}</p>
              <p><strong>Name:</strong> {selectedUser.name}</p>
              <p><strong>Email:</strong> {selectedUser.email}</p>
              <p><strong>Phone:</strong> {selectedUser.phoneNumber}</p>
              <p><strong>Address:</strong> {selectedUser.address}</p>
              <p><strong>Status:</strong> {selectedUser.isActive ? 'Active' : 'Inactive'}</p>
            </div>

            <div className="mt-6 flex justify-between gap-4">
              <button
                onClick={() => openUpdateModal(selectedUser)}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-300 transition"
              >
                Update
              </button>
              <button
                onClick={handleDelete}
                className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-300 transition"
              >
                Delete
              </button>
              <button
                onClick={closeModal}
                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-300 transition"
              >
                Close
              </button>
            </div>
          </div>
        </div>
      )}

      {isUpdateModalOpen && updateUser && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white p-6 rounded-lg w-96 shadow-lg transform transition-all scale-100 hover:scale-105">
            <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">Update User</h3>

            <div className="space-y-4">
              <div>
                <label htmlFor="name" className="block text-sm font-medium text-gray-700">Name</label>
                <input
                  type="text"
                  id="name"
                  value={updateUser.name}
                  onChange={(e) => setUpdateUser({ ...updateUser, name: e.target.value })}
                  className="w-full p-2 border rounded-lg"
                />
              </div>
              <div>
                <label htmlFor="phoneNumber" className="block text-sm font-medium text-gray-700">Phone Number</label>
                <input
                  type="text"
                  id="phoneNumber"
                  value={updateUser.phoneNumber}
                  onChange={(e) => setUpdateUser({ ...updateUser, phoneNumber: e.target.value })}
                  className="w-full p-2 border rounded-lg"
                />
              </div>
              <div>
                <label htmlFor="address" className="block text-sm font-medium text-gray-700">Address</label>
                <input
                  type="text"
                  id="address"
                  value={updateUser.address}
                  onChange={(e) => setUpdateUser({ ...updateUser, address: e.target.value })}
                  className="w-full p-2 border rounded-lg"
                />
              </div>
            </div>

            <div className="mt-6 flex justify-between gap-4">
              <button
                onClick={handleUpdate}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-300 transition"
              >
                Update
              </button>
              <button
                onClick={closeUpdateModal}
                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-300 transition"
              >
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ManageUser;
