import { useEffect, useState } from 'react'
import { reportService, alertService } from '../services/api'
import {
  Package, Truck, Tag, AlertTriangle,
  TrendingDown, DollarSign, Bell
} from 'lucide-react'
import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid,
  Tooltip, ResponsiveContainer, PieChart, Pie, Cell, Legend
} from 'recharts'

const COLORS = ['#3B82F6', '#10B981', '#F59E0B', '#EF4444', '#8B5CF6', '#EC4899']

function StatCard({ icon: Icon, label, value, color, sub }) {
  return (
    <div className="card flex items-start gap-4">
      <div className={`p-3 rounded-lg ${color}`}>
        <Icon className="w-6 h-6 text-white" />
      </div>
      <div>
        <p className="text-sm text-gray-500">{label}</p>
        <p className="text-2xl font-bold text-gray-900">{value}</p>
        {sub && <p className="text-xs text-gray-400 mt-0.5">{sub}</p>}
      </div>
    </div>
  )
}

export default function Dashboard() {
  const [stats, setStats] = useState(null)
  const [topProducts, setTopProducts] = useState([])
  const [inventoryValue, setInventoryValue] = useState([])
  const [alerts, setAlerts] = useState([])

  useEffect(() => {
    reportService.dashboard().then(r => setStats(r.data))
    reportService.topProducts().then(r => setTopProducts(r.data))
    reportService.inventoryValue().then(r => setInventoryValue(r.data))
    alertService.getAll({ resolved: false }).then(r => setAlerts(r.data.slice(0, 5)))
  }, [])

  if (!stats) return <div className="text-gray-500">Loading dashboard...</div>

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-gray-500 mt-1">Welcome back! Here's your inventory overview.</p>
      </div>

      {/* Stat Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-4">
        <StatCard icon={Package} label="Total Products" value={stats.totalProducts}
          color="bg-blue-500" />
        <StatCard icon={Truck} label="Active Suppliers" value={stats.totalSuppliers}
          color="bg-green-500" />
        <StatCard icon={TrendingDown} label="Low Stock Items" value={stats.lowStockCount}
          color="bg-yellow-500" sub={`${stats.outOfStockCount} out of stock`} />
        <StatCard icon={DollarSign} label="Inventory Value"
          value={`$${stats.totalInventoryValue.toLocaleString('en-US', { minimumFractionDigits: 0 })}`}
          color="bg-purple-500" />
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 xl:grid-cols-2 gap-6">
        {/* Top Products Bar Chart */}
        <div className="card">
          <h2 className="font-semibold text-gray-900 mb-4">Top 10 Products by Movement</h2>
          <ResponsiveContainer width="100%" height={260}>
            <BarChart data={topProducts} margin={{ top: 5, right: 5, left: 0, bottom: 60 }}>
              <CartesianGrid strokeDasharray="3 3" stroke="#F3F4F6" />
              <XAxis dataKey="sku" angle={-40} textAnchor="end" tick={{ fontSize: 11 }} />
              <YAxis tick={{ fontSize: 11 }} />
              <Tooltip formatter={(v) => [v, 'Units']} />
              <Bar dataKey="totalMovement" fill="#3B82F6" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </div>

        {/* Inventory Value Pie Chart */}
        <div className="card">
          <h2 className="font-semibold text-gray-900 mb-4">Inventory Value by Category</h2>
          <ResponsiveContainer width="100%" height={260}>
            <PieChart>
              <Pie data={inventoryValue} dataKey="totalValue" nameKey="category"
                cx="50%" cy="50%" outerRadius={90} label={({ category, percent }) =>
                  `${category} ${(percent * 100).toFixed(0)}%`}>
                {inventoryValue.map((_, i) => (
                  <Cell key={i} fill={COLORS[i % COLORS.length]} />
                ))}
              </Pie>
              <Tooltip formatter={(v) => [`$${v.toLocaleString()}`, 'Value']} />
            </PieChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* Recent Alerts */}
      <div className="card">
        <div className="flex items-center justify-between mb-4">
          <h2 className="font-semibold text-gray-900 flex items-center gap-2">
            <Bell className="w-5 h-5 text-red-500" />
            Active Alerts
          </h2>
          <span className="badge-red">{stats.activeAlerts} open</span>
        </div>
        {alerts.length === 0 ? (
          <p className="text-gray-500 text-sm">No active alerts. 🎉</p>
        ) : (
          <div className="space-y-3">
            {alerts.map(a => (
              <div key={a.id} className="flex items-start gap-3 p-3 rounded-lg bg-red-50 border border-red-100">
                <AlertTriangle className="w-5 h-5 text-red-500 flex-shrink-0 mt-0.5" />
                <div>
                  <p className="text-sm font-medium text-red-800">{a.productName}</p>
                  <p className="text-xs text-red-600">{a.message}</p>
                </div>
                <span className={a.alertType === 'OUT_OF_STOCK' ? 'badge-red ml-auto' : 'badge-yellow ml-auto'}>
                  {a.alertType.replace('_', ' ')}
                </span>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
