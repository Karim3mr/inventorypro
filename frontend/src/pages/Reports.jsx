import { useEffect, useState } from 'react'
import { reportService } from '../services/api'
import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip,
  ResponsiveContainer, LineChart, Line, PieChart, Pie, Cell
} from 'recharts'

const COLORS = ['#3B82F6','#10B981','#F59E0B','#EF4444','#8B5CF6','#EC4899']

export default function Reports() {
  const [movement, setMovement] = useState([])
  const [top, setTop] = useState([])
  const [value, setValue] = useState([])

  useEffect(() => {
    reportService.stockMovement().then(r => setMovement(r.data))
    reportService.topProducts().then(r => setTop(r.data))
    reportService.inventoryValue().then(r => setValue(r.data))
  }, [])

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Reports</h1>
        <p className="text-gray-500 text-sm">Inventory analytics and insights</p>
      </div>

      {/* Top Products */}
      <div className="card">
        <h2 className="font-semibold text-gray-900 mb-1">Top 10 Products by Movement</h2>
        <p className="text-gray-500 text-sm mb-4">Products with the highest total stock movement</p>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={top} margin={{ top: 5, right: 10, left: 0, bottom: 70 }}>
            <CartesianGrid strokeDasharray="3 3" stroke="#F3F4F6" />
            <XAxis dataKey="sku" angle={-45} textAnchor="end" tick={{ fontSize: 11 }} />
            <YAxis tick={{ fontSize: 11 }} />
            <Tooltip formatter={(v, n) => [v, n === 'totalMovement' ? 'Units' : 'Value ($)']} />
            <Bar dataKey="totalMovement" fill="#3B82F6" radius={[4,4,0,0]} name="totalMovement" />
          </BarChart>
        </ResponsiveContainer>
      </div>

      {/* Stock Movement IN vs OUT */}
      <div className="card">
        <h2 className="font-semibold text-gray-900 mb-1">Stock In vs Out by Product</h2>
        <p className="text-gray-500 text-sm mb-4">Comparison of stock received vs stock issued</p>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={movement.slice(0, 15)} margin={{ top: 5, right: 10, left: 0, bottom: 70 }}>
            <CartesianGrid strokeDasharray="3 3" stroke="#F3F4F6" />
            <XAxis dataKey="sku" angle={-45} textAnchor="end" tick={{ fontSize: 11 }} />
            <YAxis tick={{ fontSize: 11 }} />
            <Tooltip />
            <Bar dataKey="totalIn" fill="#10B981" name="Stock In" radius={[4,4,0,0]} />
            <Bar dataKey="totalOut" fill="#EF4444" name="Stock Out" radius={[4,4,0,0]} />
          </BarChart>
        </ResponsiveContainer>
      </div>

      <div className="grid grid-cols-1 xl:grid-cols-2 gap-6">
        {/* Inventory Value by Category */}
        <div className="card">
          <h2 className="font-semibold text-gray-900 mb-4">Inventory Value by Category</h2>
          <ResponsiveContainer width="100%" height={280}>
            <PieChart>
              <Pie data={value} dataKey="totalValue" nameKey="category"
                cx="50%" cy="50%" outerRadius={100}
                label={({ category, percent }) => `${category} ${(percent*100).toFixed(0)}%`}>
                {value.map((_, i) => <Cell key={i} fill={COLORS[i % COLORS.length]} />)}
              </Pie>
              <Tooltip formatter={(v) => [`$${v.toLocaleString()}`, 'Value']} />
            </PieChart>
          </ResponsiveContainer>
        </div>

        {/* Stock Movement Table */}
        <div className="card p-0 overflow-hidden">
          <div className="px-6 py-4 border-b border-gray-200">
            <h2 className="font-semibold text-gray-900">Stock Movement Summary</h2>
          </div>
          <div className="overflow-auto max-h-[280px]">
            <table className="w-full text-sm">
              <thead className="bg-gray-50 sticky top-0">
                <tr>
                  {['Product', 'In', 'Out', 'Current'].map(h => (
                    <th key={h} className="px-4 py-2 text-left text-xs font-semibold text-gray-500 uppercase">{h}</th>
                  ))}
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {movement.map((m, i) => (
                  <tr key={i} className="hover:bg-gray-50">
                    <td className="px-4 py-2 font-medium text-gray-900 max-w-[120px] truncate">{m.productName}</td>
                    <td className="px-4 py-2 text-green-600 font-medium">+{m.totalIn}</td>
                    <td className="px-4 py-2 text-red-600 font-medium">-{m.totalOut}</td>
                    <td className="px-4 py-2 font-semibold">{m.currentStock}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  )
}
