import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortUp, faSortDown } from '@fortawesome/free-solid-svg-icons';

const DataTable = ({ data, columns, sortBy, sortOrderAscending, onSortChange, onRowClick }) => {
    return (
        <table className="min-w-full table-auto border-collapse">
            <thead>
                <tr className="bg-gray-100">
                    {columns.map((column) => (
                        <th
                            key={column.field}
                            onClick={() => column.sortable && onSortChange(column.field)}
                            className={`py-2 px-4 text-left text-gray-700 border-b ${column.sortable ? 'cursor-pointer' : ''
                                }`}
                        >
                            {column.label}
                            {column.sortable && sortBy === column.field && (
                                <FontAwesomeIcon
                                    icon={sortOrderAscending ? faSortUp : faSortDown}
                                    className="ml-2"
                                />
                            )}
                        </th>
                    ))}
                </tr>
            </thead>
            <tbody>
                {data.map((row) => (
                    <tr
                        key={row.id}
                        className="hover:bg-gray-50 cursor-pointer"
                        onClick={() => onRowClick && onRowClick(row)}
                    >
                        {columns.map((column) => (
                            <td key={column.field} className="py-2 px-4 text-gray-700 border-b">
                                {column.render ? column.render(row) : row[column.field]}
                            </td>
                        ))}
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default DataTable;
