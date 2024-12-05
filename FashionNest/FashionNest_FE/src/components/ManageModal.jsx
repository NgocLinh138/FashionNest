import React from 'react';
import Swal from 'sweetalert2';

const ManageModal = ({ type, data, onClose, onDelete, onEdit, isOpen }) => {
  if (!isOpen) return null;

  const handleDelete = async () => {
    const result = await Swal.fire({
      title: 'Are you sure?',
      text: 'This action cannot be undone!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel',
    });

    if (result.isConfirmed) {
      onDelete(data.id); // Gọi hàm xóa ở đây
      Swal.fire('Deleted!', `${type} has been deleted.`, 'success');
      onClose(); // Đóng modal sau khi xóa
    }
  };

  const handleEdit = () => {
    onEdit(data); // Mở modal chỉnh sửa
    onClose(); // Đóng modal này
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
      <div className="bg-white p-6 rounded-lg w-96 shadow-lg">
        <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">
          {`View ${type}`}
        </h3>
        <div className="space-y-4">
          <p><strong>ID:</strong> {data.id}</p>
          <p><strong>Name:</strong> {data.name}</p>
          {type === 'user' && <p><strong>Email:</strong> {data.email}</p>}
          {type === 'product' && <p><strong>Price:</strong> {data.price}</p>}
        </div>
        <div className="mt-6 flex justify-between gap-4">
          <button
            onClick={() => onClose()}
            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
          >
            Close
          </button>
          <button
            onClick={handleEdit}
            className="px-4 py-2 bg-yellow-600 text-white rounded-lg hover:bg-yellow-700"
          >
            Edit
          </button>
          <button
            onClick={handleDelete}
            className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
};

export default ManageModal;
