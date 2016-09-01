namespace BlackCatList.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    public class AddressMapper
    {
        public AddressMapper(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        private ApplicationDbContext DbContext { get; }

        public async Task MapAsync(IAddressViewModel address)
        {
            int countryId = await this.MapCountryAsync(address);

            int? cityId = await this.MapCityAsync(address, countryId);

            await this.MapStreetAsync(address, cityId);
        }

        private async Task<int> MapCountryAsync(IAddressViewModel address)
        {
            if (string.IsNullOrWhiteSpace(address.Country))
            {
                throw new ArgumentException($"Country cannot be empty.");
            }

            var entity = await this.DbContext.Countries.FirstOrDefaultAsync(x => x.Name == address.Country);
            if (entity == null)
            {
                throw new ArgumentException($"Country '{address.Country}' does not exist.");
            }

            return address.CountryId = entity.Id;
        }

        private async Task<int?> MapCityAsync(IAddressViewModel address, int countryId)
        {
            if (string.IsNullOrWhiteSpace(address.City))
            {
                return null as int?;
            }

            var entity = await this.DbContext.Cities.FirstOrDefaultAsync(x => x.Name == address.City && x.CountryId == countryId);
            if (entity == null)
            {
                entity = this.DbContext.Cities.Add(new City
                {
                    Name = address.City,
                    CountryId = countryId
                });

                await this.DbContext.SaveChangesAsync();
            }

            return address.CityId = entity.Id;
        }

        private async Task MapStreetAsync(IAddressViewModel address, int? cityId)
        {
            if (string.IsNullOrWhiteSpace(address.Street))
            {
                return;
            }

            if (cityId == null)
            {
                throw new ArgumentNullException(nameof(cityId));
            }

            var entity = await this.DbContext.Streets.FirstOrDefaultAsync(x => x.Name == address.Street && x.CityId == cityId);
            if (entity == null)
            {
                entity = this.DbContext.Streets.Add(new Street
                {
                    Name = address.Street,
                    CityId = cityId.Value
                });

                await this.DbContext.SaveChangesAsync();
            }

            address.StreetId = entity.Id;
        }
    }
}