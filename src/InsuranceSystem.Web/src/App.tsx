import { useEffect, useState } from 'react';
import axios from 'axios';

interface InsuranceReport {
  averageVehicleValue: number;
  averageCommercialPremium: number;
  totalInsurances: number;
}

function App() {
  const [report, setReport] = useState<InsuranceReport | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchReport = () => {
    setLoading(true);
    setError(null);
    const apiUrl = 'http://localhost:5000/api/Insurance/report';

    axios.get(apiUrl)
      .then(response => {
        setReport(response.data);
        setLoading(false);
      })
      .catch(err => {
        console.error("Error fetching report:", err);
        setError("Failed to load report. The API might be starting up. Please try again in a few seconds.");
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchReport();
  }, []);

  return (
    <div className="container mt-5">
      <div className="card shadow">
        <div className="card-header bg-primary text-white">
          <h1 className="h3 mb-0">Insurance Report</h1>
        </div>
        <div className="card-body">
          {loading && (
            <div className="text-center py-5">
              <div className="spinner-border text-primary" role="status">
                <span className="visually-hidden">Loading...</span>
              </div>
              <p className="mt-2 text-muted">Connecting to API...</p>
            </div>
          )}

          {error && (
            <div className="text-center py-3">
              <div className="alert alert-warning" role="alert">
                {error}
              </div>
              <button className="btn btn-primary" onClick={fetchReport}>
                Try Again
              </button>
            </div>
          )}

          {report && (
            <div className="row text-center">
              <div className="col-md-4 mb-3">
                <div className="card bg-light border-primary h-100">
                  <div className="card-body">
                    <h5 className="card-title text-primary">Total Insurances</h5>
                    <p className="display-4 fw-bold">{report.totalInsurances}</p>
                  </div>
                </div>
              </div>
              <div className="col-md-4 mb-3">
                <div className="card bg-light border-success h-100">
                  <div className="card-body">
                    <h5 className="card-title text-success">Avg Vehicle Value</h5>
                    <p className="display-6 fw-bold">
                      {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(report.averageVehicleValue)}
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-4 mb-3">
                <div className="card bg-light border-info h-100">
                  <div className="card-body">
                    <h5 className="card-title text-info">Avg Commercial Premium</h5>
                    <p className="display-6 fw-bold">
                      {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(report.averageCommercialPremium)}
                    </p>
                  </div>
                </div>
              </div>
            </div>
          )}
          
          {!loading && !report && !error && (
            <p className="text-center text-muted">No data available.</p>
          )}
        </div>
        <div className="card-footer text-muted text-center">
           Generated on {new Date().toLocaleDateString()}
        </div>
      </div>
    </div>
  );
}

export default App;