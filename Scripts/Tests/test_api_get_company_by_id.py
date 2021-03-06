import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_company(company):
    v = Validator(api_schemas.company_schema)
    correct = v.validate(company)
    assert correct == True

def f_test_company_id(company, id):
    correct = True
    if company["id"] != id:
        correct = False

    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200


base_url = ''
companies_path = "companies"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test single company
def test_company():
    id=1
    company_request = requests.get(base_url+companies_path+"/"+str(id))
    f_test_status_code_200(company_request)
    company = company_request.json()
    f_test_company(company)
    f_test_company_id(company, id)