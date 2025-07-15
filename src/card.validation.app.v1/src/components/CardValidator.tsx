import React, { useState } from 'react';
import { CardValidationResultDto } from '../types/CardValidationResultDto';

const CardValidator: React.FC = () => {
  const [cardNumber, setCardNumber] = useState('');
  const [result, setResult] = useState<CardValidationResultDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleValidate = async () => {
    setLoading(true);
    setResult(null);
    setError(null);

    try {
      const res = await fetch(`${process.env.REACT_APP_API_BASE_URL}/api/cards/validate`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ cardNumber }),
      });

      if (!res.ok) {
        throw new Error(`Server responded with status ${res.status}`);
      }

      const data: CardValidationResultDto = await res.json();
      setResult(data);
    } catch (err: any) {
      setError(err.message || 'Unknown error');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container py-5">
      <div className="row justify-content-center">
        <div className="col-md-6">
          <div className="card shadow-sm border-0">
            <div className="card-header bg-primary text-white">
              <h5 className="mb-0">Credit Card Validator</h5>
            </div>
            <div className="card-body">
              <div className="mb-3">
                <label htmlFor="cardNumber" className="form-label">
                  Enter Card Number
                </label>
                <input
                  id="cardNumber"
                  className="form-control"
                  value={cardNumber}
                  onChange={(e) => setCardNumber(e.target.value)}
                  placeholder="e.g. 4111111111111111"
                />
              </div>

              <button
                className="btn btn-success w-100"
                onClick={handleValidate}
                disabled={!cardNumber || loading}
              >
                {loading ? 'Validating...' : 'Validate Card'}
              </button>

              {result && (
                <div className="alert alert-info mt-4">
                  <p><strong>Card Type:</strong> {result.cardType}</p>
                  <p><strong>Valid:</strong> {result.isValid ? '✅ Yes' : '❌ No'}</p>
                  <p><strong>Formatted:</strong> {result.formattedNumber}</p>
                </div>
              )}

              {error && (
                <div className="alert alert-danger mt-4">
                  {error}
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CardValidator;