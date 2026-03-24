# 🛡️ DataSentinel: AI-Powered KVKK Scanner

DataSentinel is a robust .NET Web API project designed to detect **Personally Identifiable Information (PII)** within PDF documents and perform automated **KVKK/GDPR risk assessments** using the **Gemini 3.1 Flash** AI model.

> [!TIP]
> **New Update:** Monitor your scans in real-time with the modern **Tailwind CSS Dashboard**, accessible on port 5000! 🚀

## 🚀 Key Features
- **Regex-Based Detection:** Automatically identifies and masks sensitive data such as Turkish ID Numbers (TCKN).
- **AI-Driven Risk Analysis:** Leverages Gemini 3.1 Flash to evaluate data risk (Scoring from 1-10).
- **Asynchronous Background Processing:** Efficiently handles document tasks using a thread-safe Queue architecture.
- **Database Persistence:** All scan results and AI summaries are securely stored via SQLite and EF Core.

## 🛠️ Technology Stack
- **Backend:** .NET 8.0 Web API
- **AI Integration:** Google Gemini AI API (3.1 Flash)
- **Database:** Entity Framework Core (SQLite)
- **PDF Processing:** PdfPig (High-performance PDF parsing)
- **Frontend:** Tailwind CSS & Fetch API

## ⚙️ Installation & Setup
1. Clone the repository to your local machine.
2. Rename `appsettings.Template.json` to `appsettings.json`.
3. Insert your **Gemini API Key** (obtained from Google AI Studio) into the configuration.
4. Build and run the project using the command: `dotnet run` (Default Port: 5000).

## 📖 Usage Guide & Dashboard

DataSentinel processes documents using an asynchronous queue system:

1. **Data Input:** Copy your PDF documents into the `Uploads` folder located in the project root. (The folder is automatically created on the first run).
2. **Automated Processing:** The `ProcessingWorker` detects new files instantly and initiates the AI-powered KVKK analysis.
3. **Visualization:** Access `http://localhost:5000` to view the Dashboard. Here, you can track scan status, risk scores, and detailed AI-generated summaries.