using backendPrueba.Models;
using backendPrueba.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRespository _customerRepository;

        public CustomerController(ICustomerRespository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("get-all-customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerById(id);

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el cliente: {ex.Message}");
            }
        }

        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            try
            {
                await _customerRepository.CreateCustomer(customer);
                return StatusCode(StatusCodes.Status201Created, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error al crear el cliente: {ex.Message}" });
            }
        }

        [HttpPut("update-customer/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerById(id);
                if (existingCustomer == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado." });
                }

                customer.Id = id;

                await _customerRepository.UpdateCustomer(customer);
                return Ok(new { message = $"Cliente con ID {id} actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error al actualizar el cliente: {ex.Message}" });
            }
        }

        [HttpDelete("delete-customer/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerById(id);
                if (existingCustomer == null)
                {
                    return NotFound(new { message = $"Cliente con el {id} no encontrado." });
                }
                await _customerRepository.DeleteCustomer(id);
                return Ok(new { message = $"Cliente con {id} eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error al eliminar el cliente: {ex.Message}" });
            }
        }


    }
}
