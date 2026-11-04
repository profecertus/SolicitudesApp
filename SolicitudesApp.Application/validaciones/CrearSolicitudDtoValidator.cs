using FluentValidation;
using SolicitudesApp.Application.DTOs;

namespace SolicitudesApp.Application.validaciones
{
    public class CrearSolicitudDtoValidator : AbstractValidator<CrearSolicitudDto>
    {
        public CrearSolicitudDtoValidator()
        {
            RuleFor(x => x.Motivo)
                .NotEmpty().WithMessage("El motivo es obligatorio.")
                .MinimumLength(5).WithMessage("El motivo debe tener al menos 5 caracteres")
                .MaximumLength(250).WithMessage("El motivo no puede exceder los 250 caracteres.");

            RuleFor(x => x.SolicitanteId)
                .GreaterThan(0).WithMessage("El IDE del solicitante debe ser un entero positivo");

            RuleFor(x => x.Detalles)
                .NotNull().WithMessage("Debe incluir la lista de detalles.")
                .Must(d => d != null && d.Count > 0).WithMessage("Debe registrar la menos un ítem de gasto");

            RuleForEach(x => x.Detalles).SetValidator(new CrearDetalleDtoValidator());
       
        }
    }

    public class CrearDetalleDtoValidator : AbstractValidator<CrearDetalleDto>
    {
        public CrearDetalleDtoValidator()
        {
            RuleFor(x => x.Concepto) 
                .NotEmpty().WithMessage("El concepto del gasto es obligatorio")
                .MinimumLength(3).WithMessage("El concepto debe tener al menos 3 caracteres.");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0");

            RuleFor(x => x.PrecioUnitario)
                .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a 0");
        }
    }
}
