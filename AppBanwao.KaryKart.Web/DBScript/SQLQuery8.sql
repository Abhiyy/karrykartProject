--use Karrykart
--select * from UserLogin
select * from Users
select * from UserDetail
select * from UserAddressDetail
select * from OTPHolder

delete from UserAddressDetail where AddressID =18
delete from UserDetail where UserDetailsID =18
delete from Users where UserID = '559B9891-FE58-49DD-A357-B7ADC2CC339A'
delete from OTPHolder where OTPID=21