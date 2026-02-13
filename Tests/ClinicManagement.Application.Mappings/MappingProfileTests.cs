using Xunit;
using AutoMapper;
using ClinicManagement.Application.Mappings;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using System;

namespace Tests.ClinicManagement.Application.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_Configuration_ShouldBeValid()
    {
        // Arrange & Act & Assert
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void MappingProfile_UserToUserDto_ShouldMap()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            BirthDate = new DateTime(1990, 1, 1),
            PhoneNo = "1234567890",
            Gender = "Male",
            Address = "123 Main St",
            UserType = UserType.Patient,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        // Act
        var dto = _mapper.Map<UserDto>(user);

        // Assert
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Name, dto.Name);
        Assert.Equal(user.Email, dto.Email);
        Assert.Equal(user.BirthDate, dto.BirthDate);
        Assert.Equal(user.PhoneNo, dto.PhoneNo);
        Assert.Equal(user.Gender, dto.Gender);
        Assert.Equal(user.Address, dto.Address);
        Assert.Equal(user.UserType, dto.UserType);
        Assert.Equal(user.IsActive, dto.IsActive);
        Assert.Equal(user.CreatedDate, dto.CreatedDate);
    }

    [Fact]
    public void MappingProfile_UserCreateDtoToUser_ShouldMap()
    {
        // Arrange
        var createDto = new UserCreateDto
        {
            Name = "Jane Doe",
            Email = "jane@example.com",
            Password = "password123",
            BirthDate = new DateTime(1995, 5, 15),
            PhoneNo = "9876543210",
            Gender = "Female",
            Address = "456 Elm St",
            UserType = UserType.Doctor
        };

        // Act
        var user = _mapper.Map<User>(createDto);

        // Assert
        Assert.Equal(createDto.Name, user.Name);
        Assert.Equal(createDto.Email, user.Email);
        Assert.Equal(createDto.Password, user.Password);
        Assert.Equal(createDto.BirthDate, user.BirthDate);
        Assert.Equal(createDto.PhoneNo, user.PhoneNo);
        Assert.Equal(createDto.Gender, user.Gender);
        Assert.Equal(createDto.Address, user.Address);
        Assert.Equal(createDto.UserType, user.UserType);
    }

    [Fact]
    public void MappingProfile_UserUpdateDtoToUser_ShouldMap()
    {
        // Arrange
        var updateDto = new UserUpdateDto
        {
            Name = "Updated Name",
            Email = "updated@example.com",
            BirthDate = new DateTime(1992, 3, 20),
            PhoneNo = "5555555555",
            Gender = "Male",
            Address = "789 Oak St"
        };

        // Act
        var user = _mapper.Map<User>(updateDto);

        // Assert
        Assert.Equal(updateDto.Name, user.Name);
        Assert.Equal(updateDto.Email, user.Email);
        Assert.Equal(updateDto.BirthDate, user.BirthDate);
        Assert.Equal(updateDto.PhoneNo, user.PhoneNo);
        Assert.Equal(updateDto.Gender, user.Gender);
        Assert.Equal(updateDto.Address, user.Address);
    }
}
