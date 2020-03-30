create procedure pormote
@Studies varchar(20),
@Semester int
as
begin
if (EXISTS  (select * from Enrollment e join Studies st on e.IdStudy = st.IdStudy where e.Semester=(@Semester+1) and st.Name=@Studies))
	begin
		update Student set IdEnrollment=(select IdEnrollment from Enrollment e join Studies st on e.IdStudy = st.IdStudy where e.Semester=(@Semester+1) and st.Name=@Studies) where IdEnrollment=@Semester;
	end;
else
	begin
		insert into Enrollment values ((select max(idEnrollment)+1 from Enrollment),(@Semester+1),(select idStudy from Studies where Name=@Studies), GETDATE());
		update Student set IdEnrollment=(select max(IdEnrollment) from Enrollment) where IdEnrollment=@Semester;
	end;
end;