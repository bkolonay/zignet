import React, { Component } from 'react';
import { getDefaultFilter } from '../common/routing/SearchFilter.js'
import FilterCheckbox from './FilterCheckbox'

class Filter extends Component {
	constructor(props) {
		super(props)

		this.handleClear = this.handleClear.bind(this);

		this.applications = [			
			{ Id: 1, Name: "LoopNet" },
			{ Id: 2, Name: "LmMobile", Label: "LM Mobile" },
			{ Id: 3, Name: "CityFeet" },
			{ Id: 4, Name: "Showcase" }
		];

		this.environments = [			
			{ Id: 1, Name: "DVM" },
			{ Id: 2, Name: "TSM" },
			{ Id: 3, Name: "TSR" },
			{ Id: 4, Name: "Prod" },
			{ Id: 5, Name: "Dev" },
			{ Id: 6, Name: "Test" },
		];
		
		this.suites = [			
			{ Id: 1, Name: "UI" },
			{ Id: 2, Name: "Services" },
			{ Id: 3, Name: "MobileServices", Label:"Mobile Services" }
		];		
	}

	handleClear(event) {
		event.preventDefault();
		this.props.onFilterChange(getDefaultFilter());
	}

  render() {
    return (
      <div className="row">
      	<div className="col">
          {this.applications.map((application) =>
            <FilterCheckbox key={application.Id} filter={this.props.filter} name={application.Name} label={application.Label} onFilterChange={this.props.onFilterChange} />
          )}
				</div>
      	<div className="col">
          {this.environments.map((environment) =>
            <FilterCheckbox key={environment.Id} filter={this.props.filter} name={environment.Name} label={environment.Label} onFilterChange={this.props.onFilterChange} />
          )}
				</div>
      	<div className="col">
          {this.suites.map((suite) =>
            <FilterCheckbox key={suite.Id} filter={this.props.filter} name={suite.Name} label={suite.Label} onFilterChange={this.props.onFilterChange} />
          )}
				</div>
				<div className="col">
					<button onClick={this.handleClear} type="button" className="btn btn-link">Clear filters</button>
				</div>
      </div>
    );
  }
}

export default Filter;
