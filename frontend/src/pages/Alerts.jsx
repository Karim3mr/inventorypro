import { useEffect, useState } from 'react'
import { alertService } from '../services/api'
import toast from 'react-hot-toast'
import { AlertTriangle, CheckCircle, Bell } from 'lucide-react'
import { useAuth } from '../context/AuthContext'

export default function Alerts() {
  const { user } = useAuth()
  const canResolve = ['Admin', 'Manager'].includes(user?.role)
  const [alerts, setAlerts] = useState([])
  const [filter, setFilter] = useState('false')

  const load = () => alertService.getAll({ resolved: filter === 'all' ? undefined : filter === 'true' })
    .then(r => setAlerts(r.data))

  useEffect(() => { load() }, [filter])

  const resolve = async (id) => {
    await alertService.resolve(id)
    toast.success('Alert resolved!'); load()
  }

  const active = alerts.filter(a => !a.isResolved)
  const resolved = alerts.filter(a => a.isResolved)

  return (
    <div className="space-y-5">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Alerts</h1>
          <p className="text-gray-500 text-sm">{active.length} active alerts</p>
        </div>
        <div className="flex gap-2">
          {['false', 'true', 'all'].map(v => (
            <button key={v}
              className={`px-3 py-1.5 rounded-lg text-sm font-medium transition-colors ${filter === v ? 'bg-blue-600 text-white' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'}`}
              onClick={() => setFilter(v)}>
              {v === 'false' ? 'Active' : v === 'true' ? 'Resolved' : 'All'}
            </button>
          ))}
        </div>
      </div>

      {alerts.length === 0 ? (
        <div className="card text-center py-16">
          <Bell className="w-12 h-12 text-gray-300 mx-auto mb-3" />
          <p className="text-gray-500">No alerts found.</p>
        </div>
      ) : (
        <div className="space-y-3">
          {alerts.map(a => (
            <div key={a.id} className={`card flex items-start gap-4 ${a.isResolved ? 'opacity-60' : ''}`}>
              <div className={`p-2 rounded-lg ${a.alertType === 'OUT_OF_STOCK' ? 'bg-red-100' : 'bg-yellow-100'}`}>
                <AlertTriangle className={`w-5 h-5 ${a.alertType === 'OUT_OF_STOCK' ? 'text-red-600' : 'text-yellow-600'}`} />
              </div>
              <div className="flex-1 min-w-0">
                <div className="flex items-center gap-2 flex-wrap">
                  <span className="font-semibold text-gray-900">{a.productName}</span>
                  <span className="font-mono text-xs text-gray-500">{a.sku}</span>
                  <span className={a.alertType === 'OUT_OF_STOCK' ? 'badge-red' : 'badge-yellow'}>
                    {a.alertType.replace('_', ' ')}
                  </span>
                  {a.isResolved && <span className="badge-green">Resolved</span>}
                </div>
                <p className="text-gray-600 text-sm mt-1">{a.message}</p>
                <p className="text-gray-400 text-xs mt-1">{new Date(a.createdAt).toLocaleString()}</p>
              </div>
              {canResolve && !a.isResolved && (
                <button onClick={() => resolve(a.id)}
                  className="flex items-center gap-1.5 text-sm text-green-600 hover:text-green-700 font-medium flex-shrink-0">
                  <CheckCircle className="w-4 h-4" />Resolve
                </button>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  )
}
