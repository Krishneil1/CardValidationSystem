import React, { useState } from "react";

interface CardValidationResultDto {
  cardType: string;
  isValid: boolean;
  formattedNumber: string;
}

const CardValidator: React.FC = () => {
  const [cardNumber, setCardNumber] = useState("");
  const [result, setResult] = useState<CardValidationResultDto | null>(null);
  const [error, setError] = useState<string | null>(null);

  const handleValidate = async () => {
    setResult(null);
    setError(null);

    try {
      const response = await fetch(`${process.env.REACT_APP_API_BASE_URL}/api/cards/validate`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ cardNumber }),
      });

      if (!response.ok) {
        throw new Error("API request failed");
      }

      const data: CardValidationResultDto = await response.json();
      setResult(data);
    } catch (err) {
      setError("Failed to validate card. Please try again.");
    }
  };

  return (
    <div className="card card-validator-wrapper shadow-sm p-4 bg-white rounded">
      <div className="card-header bg-primary text-white text-center fw-bold">
        Credit Card Validator
      </div>

      <div className="card-body">
        <div className="mb-3">
          <label htmlFor="cardNumber" className="form-label">Enter Card Number</label>
          <input
            id="cardNumber"
            type="text"
            className="form-control"
            value={cardNumber}
            onChange={(e) => setCardNumber(e.target.value)}
          />
        </div>

        <button className="btn btn-success w-100" onClick={handleValidate}>
          Validate Card
        </button>

        <div className={`result-box ${result || error ? "show" : ""}`}>
          {error && (
            <div className="result-fail mt-3">{error}</div>
          )}

          {result && (
            <div
              className={
                result.cardType === "Unknown"
                  ? "result-unknown"
                  : result.isValid
                  ? "result-success"
                  : "result-fail"
              }
            >
              <p><strong>Card Type:</strong> {result.cardType}</p>
              <p><strong>Valid:</strong> {result.isValid ? "✅ Yes" : "❌ No"}</p>
              <p><strong>Formatted:</strong> {result.formattedNumber}</p>
            </div>
          )}

        </div>
      </div>
    </div>
  );
};

export default CardValidator;
