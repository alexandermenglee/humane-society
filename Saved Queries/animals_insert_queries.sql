insert into dbo.Animals(Name, Weight, Age, Demeanor, KidFriendly, PetFriendly, Gender, AdoptionStatus, CategoryId, DietPlanId, EmployeeId)
values('Fluffy', 87, 7, 'Aggressive', 0, 0, 'Female', 'Not Adopted', 2, 3, 4)

select * from dbo.Animals

insert into dbo.Animals(Name, Weight, Age, Demeanor, KidFriendly, PetFriendly, Gender, AdoptionStatus, CategoryId, DietPlanId, EmployeeId)
values('Rocky', 27, 9, 'Curious', 1, 1, 'Male', 'Not Adopted', 3, 1, 2),
('Bubbles', 21, 5, 'Friendly', 0, 1, 'Female', 'Adopted', 2, 3, 4),
('Victor', 45, 21, 'Nervous', 1, 0, 'Male', 'Not Adopted', 4, 2, 1),
('Spock', 102, 2, 'Aggressive', 0, 0, 'Male', 'Not Adopted', 5, 5, 5)

select * from dbo.Animals