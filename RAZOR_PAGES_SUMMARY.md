# Razor Pages Structure Summary

## Overview
All Razor Pages have been created for the ClinicManagement.Web application with proper ASP.NET Core patterns, model binding, validation, and error handling.

## File Structure

### Root Configuration Files
- `/Pages/_ViewStart.cshtml` - Sets default layout for all pages
- `/Pages/_ViewImports.cshtml` - Imports common namespaces and tag helpers

### Shared Layout Pages (/Pages/Shared/)
- `_Layout.cshtml` - Main layout with Bootstrap 5, responsive navigation menu with Patient, Doctor, and Admin sections
- `_ValidationScriptsPartial.cshtml` - Client-side validation scripts (jQuery Validate)
- `Error.cshtml` + `Error.cshtml.cs` - Global error page with proper logging and error display

### Home Page (/Pages/)
- `Index.cshtml` + `Index.cshtml.cs` - Dashboard with statistics, recent activity, and quick actions

### Patient Pages (/Pages/Patients/)
- `Index.cshtml` + `Index.cshtml.cs` - Patient list with search functionality
- `Create.cshtml` + `Create.cshtml.cs` - Create new patient with validation
- `Edit.cshtml` + `Edit.cshtml.cs` - Edit existing patient
- `Details.cshtml` + `Details.cshtml.cs` - View patient details with age calculation
- `Delete.cshtml` + `Delete.cshtml.cs` - Delete patient with confirmation

### Doctor Pages (/Pages/Doctors/)
- `Index.cshtml` + `Index.cshtml.cs` - Doctor list with search functionality
- `Create.cshtml` + `Create.cshtml.cs` - Create new doctor with validation
- `Edit.cshtml` + `Edit.cshtml.cs` - Edit existing doctor
- `Details.cshtml` + `Details.cshtml.cs` - View doctor details with statistics
- `Delete.cshtml` + `Delete.cshtml.cs` - Delete doctor with confirmation

## Key Features

### Bootstrap 5 Integration
- Responsive design with mobile-first approach
- Bootstrap Icons for visual enhancement
- Card-based layouts for better organization
- Alert messages for user feedback (success, error, info)

### Navigation Menu
- Patient section: View all patients, Add new patient
- Doctor section: View all doctors, Add new doctor
- Admin section: Reports, Audit logs, Settings (placeholder links)
- Search functionality in navbar

### Model Binding & Validation
- All forms use proper model binding with `[BindProperty]`
- Data annotations for validation (Required, StringLength, EmailAddress, Phone, etc.)
- Client-side validation with jQuery Validate
- Server-side validation with ModelState
- Custom display names with `[Display]` attribute

### Error Handling
- Try-catch blocks in all page handlers
- Structured logging with ILogger
- TempData for cross-page messages
- User-friendly error messages
- Development vs Production error details

### Search Functionality
- Patient search by name and email
- Doctor search by name, email, and specialization
- Case-insensitive search using StringComparison.OrdinalIgnoreCase

### User Experience
- Confirmation dialogs for delete operations
- Success/error messages using TempData
- Loading states and empty states
- Breadcrumb navigation
- Action buttons with icons
- Responsive tables with action buttons

### Security & Best Practices
- CSRF protection (automatic with Razor Pages)
- Input validation on both client and server
- SQL injection prevention (through EF Core parameterization)
- Proper HTTP verb usage (GET for display, POST for modifications)
- Logging for audit trails

## Patient Form Fields
- First Name (required, max 100 chars)
- Last Name (required, max 100 chars)
- Date of Birth (required, date picker)
- Gender (required, dropdown: Male/Female/Other)
- Email (required, email validation)
- Phone Number (required, phone validation)
- Address (required, max 500 chars, textarea)
- Medical History (optional, max 2000 chars, textarea)

## Doctor Form Fields
- First Name (required, max 100 chars)
- Last Name (required, max 100 chars)
- Specialization (required, dropdown with common specializations)
- License Number (required, max 50 chars)
- Email (required, email validation)
- Phone Number (required, phone validation)
- Is Active (checkbox, default true)

## Dashboard Features
- Statistics cards for Patients, Doctors, and Appointments
- Recent activity showing last 5 registered patients and doctors
- Quick action buttons for common tasks
- System information display

## File Locations
All files are located in: `/modernize-data/studio-data/TNT1001/APP1460/transformed-code/493/studio-workspace/upgrade-rah/src/ClinicManagement.Web/Pages/`

## Next Steps
1. Create wwwroot/css/site.css for custom styles
2. Create wwwroot/js/site.js for custom JavaScript
3. Update Program.cs to configure Razor Pages
4. Test all pages end-to-end
5. Add additional features (appointments, medical records, etc.)
