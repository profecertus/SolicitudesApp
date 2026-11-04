using FluentValidation;
using SolicitudesApp.Application.DTOs;

namespace SolicitudesApp.Application.validaciones
{
    public class CrearCentroCostoDtoValidator : AbstractValidator<CrearCentroCostoDto>
    {
        public CrearCentroCostoDtoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("El código es obligatorio.")
                .MaximumLength(20).WithMessage("El código no puede exceder los 20 caracteres.");
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
        }
    }

    public class ActualizarCentroCostoDtoValidator : AbstractValidator<ActualizarCentroCostoDto>
    {
        public ActualizarCentroCostoDtoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("El código es obligatorio.")
                .MaximumLength(20).WithMessage("El código no puede exceder los 20 caracteres.");
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
        }
    }

    public class GuardarRolDtoValidator : AbstractValidator<GuardarRolDto>
    {
        public GuardarRolDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre del rol no puede exceder los 50 caracteres.");
            RuleFor(x => x.LimiteAprobacion)
                .GreaterThanOrEqualTo(0).WithMessage("El límite de aprobación no puede ser negativo.");
        }
    }

    public class CrearUsuarioDtoValidator : AbstractValidator<CrearUsuarioDto>
    {
        public CrearUsuarioDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");
            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.");
        }
    }

    public class ActualizarUsuarioDtoValidator : AbstractValidator<ActualizarUsuarioDto>
    {
        public ActualizarUsuarioDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");
            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.");
        }
    }

    public class GuardarDetalleDtoValidator : AbstractValidator<GuardarDetalleDto>
    {
        public GuardarDetalleDtoValidator()
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
